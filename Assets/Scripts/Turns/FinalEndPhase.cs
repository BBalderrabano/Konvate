using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Turns/Phases/Final Phase")]
public class FinalEndPhase : Phase
{
    List<CardEffect> turn_end = new List<CardEffect>();

    bool CheckVictoryConditions()
    {
        bool localDied = GM.localPlayer.IsDead();
        bool opponentDied = GM.clientPlayer.IsDead();

        if (localDied || opponentDied)
        {
            if (localDied && !opponentDied)
            {
                EndGameScreen.singleton.EndGame(false, "");
            }
            else if (!localDied && opponentDied)
            {
                EndGameScreen.singleton.EndGame(true, "");

            }
            else if (localDied && opponentDied)
            {
                bool isOffensive = GM.turn.IsOffensivePlayer(GM.localPlayer.photonId);

                EndGameScreen.singleton.EndGame(isOffensive, (isOffensive ? "Victoria por ofensiva" : "Derrota por ofensiva"));
            }

            return false;
        }
        else
        {
            return true;
        }
    }

    void LoadCardEffects()
    {
        for (int i = 0; i < GM.turn.endTurnEffects.Count; i++)
        {
            if (GM.turn.endTurnEffects[i].isDone)
                continue;

            if (GM.turn.endTurnEffects[i].type == EffectType.ENDTURN)
            {
                turn_end.Add(GM.turn.endTurnEffects[i]);
            }
        }

        turn_end = turn_end.OrderBy(a => a.priority).ToList();
    }

    void ExecuteEffects()
    {
        foreach (CardEffect eff in turn_end)
        {
            if (eff.isDone) { continue; }

            Action turn_end = new A_ExecuteEffect(eff.card.instanceId, eff.effectId, eff.card.owner.photonId);
            GM.actionManager.AddAction(turn_end);
        }
    }

    void SyncAllCards()
    {
        Action sync_all_cards = new A_SyncronizeCards(GM.localPlayer.photonId, true, true, true, true);
        GM.actionManager.AddAction(sync_all_cards);
    }

    bool localIsDone = false;

    public override bool IsComplete()
    {
        if (isInit && GM.actionManager.IsDone())
        {
            if (GM.isMultiplayer)
            {
                if (!localIsDone)
                {
                    MultiplayerManager.singleton.PlayerIsReady();

                    GM.ChangeTurnController(GM.GetOpponentHolder(GM.localPlayer.photonId).photonId, true);
                }

                if (MultiplayerManager.singleton.ArePlayersReady())
                {
                    return CheckVictoryConditions();
                }
            }
            else
            {
                return CheckVictoryConditions();
            }
        }

        return false;
    }

    public override void OnStartPhase()
    {
        base.OnStartPhase();

        if (!isInit)
        {
            localIsDone = false;

            turn_end.Clear();

            GM.currentPlayer = null;
            GM.SetState(null);
            GM.onPhaseChange.Raise();
            GM.onPhaseControllerChange.Raise();

            LoadCardEffects();

            ExecuteEffects();

            SyncAllCards();

            isInit = true;
        }
    }

    public override void OnEndPhase()
    {
        if (isInit)
        {
            GM.turn.endTurnEffects.Clear();
            GM.SetState(null);
            isInit = false;
        }
    }
    public override bool CanPlayCard(Card c)
    {
        return false;
    }

    public override void OnPhaseControllerChange(int photonId)
    {
    }

    public override void OnTurnButtonHold()
    {
    }

    public override void OnTurnButtonPress()
    {
    }

    public override void OnPlayCard(Card c)
    {
    }
}
