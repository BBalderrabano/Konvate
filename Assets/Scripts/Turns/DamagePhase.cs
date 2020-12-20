using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Turns/Phases/Damage Phase")]
public class DamagePhase : Phase
{
    List<CardEffect> restore = new List<CardEffect>();

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

            OnEndPhase();

            return false;
        }
        else
        {
            return true;
        }
    }

    void LoadCardEffects()
    {
        int offensivePhotonId = GM.turn.offensivePlayer.photonId;

        foreach (PlayerHolder player in GM.allPlayers)
        {
            foreach (Card c in player.playedCards)
            {
                if (c.isBroken)
                    continue;

                for (int i = 0; i < c.cardEffects.Count; i++)
                {
                    if (c.cardEffects[i].isDone)
                        continue;

                    if (c.cardEffects[i].type == EffectType.RESTORE)
                    {
                        restore.Add(c.cardEffects[i]);
                    }
                }
            }

            foreach (Card c in player.handCards)
            {
                foreach (CE_HandCheck hand_effect in c.cardEffects.OfType<CE_HandCheck>())
                {
                    if (hand_effect.linkedCardEffect.isDone)
                        continue;

                    if (hand_effect.linkedCardEffect.type == EffectType.RESTORE)
                    {
                        restore.Add(hand_effect.linkedCardEffect);
                    }
                }
            }
        }

        restore = restore.OrderBy(a => a.priority).ThenBy(a => (a.card.owner.photonId != offensivePhotonId)).ToList();
    }

    void ExecuteEffects()
    {
        foreach (CardEffect eff in restore)
        {
            if (eff.isDone) { continue; }

            KAction restore_effect = new A_ExecuteEffect(eff.card.instanceId, eff.effectId, eff.card.owner.photonId);
            GM.actionManager.AddAction(restore_effect);
        }
    }

    public override bool IsComplete()
    {
        if(isInit && GM.actionManager.IsDone())
        {
            return CheckVictoryConditions();
        }
        else
        {
            return false;
        }
    }

    public override void OnStartPhase()
    {
        base.OnStartPhase();

        if (!isInit)
        {
            GM.currentPlayer = null;
            GM.SetState(null);
            GM.onPhaseChange.Raise();
            GM.onPhaseControllerChange.Raise();

            restore.Clear();

            LoadCardEffects();

            KAction checkFloatingDefend = new A_CheckFloatingDefense(GM.localPlayer.photonId);
            GM.actionManager.AddAction(checkFloatingDefend);

            KAction replaceForBleed = new A_ReplacePlayedChipsForBleed(GM.localPlayer.photonId);
            GM.actionManager.AddAction(replaceForBleed);

            KAction moveBleedToDamage = new A_MoveBleedToDamage(GM.localPlayer.photonId);
            GM.actionManager.AddAction(moveBleedToDamage);

            ExecuteEffects();

            isInit = true;
        }
    }

    public override void OnEndPhase()
    {
        if (isInit)
        {
            GameManager.singleton.SetState(null);
            isInit = false;
        }
    }

    public override bool CanPlayCard(Card c)
    {
        return false;
    }

    public override void OnTurnButtonPress(Button button)
    {
    }

    public override void OnTurnButtonHold(Button button)
    {
    }

    public override void OnPhaseControllerChange(int photonId)
    {
    }

    public override void OnPlayCard(Card c)
    {
    }
}
