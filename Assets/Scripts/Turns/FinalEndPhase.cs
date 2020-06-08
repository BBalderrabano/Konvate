using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Turns/Phases/Final Phase")]
public class FinalEndPhase : Phase
{
    List<CardEffect> turn_end = new List<CardEffect>();
    public SO.GameEvent PhaseControllerChangeEvent;

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
        if (GM.isMultiplayer)
        {
            foreach (PlayerHolder player in GM.allPlayers)
            {
                foreach (Card c in player.playedCards)
                {
                    for (int i = 0; i < c.cardEffects.Count; i++)
                    {
                        if (c.cardEffects[i].isDone)
                            continue;

                        if (c.cardEffects[i].type == EffectType.ENDTURN)
                        {
                            turn_end.Add(c.cardEffects[i]);
                        }
                    }
                }
            }

            turn_end = turn_end.OrderBy(a => a.priority).ToList();
        }
    }

    bool executing = false;
    int effectIndex = 0;

    void ExecuteEffect()
    {
        if (!turn_end[effectIndex].isDone && !executing)
        {
            executing = true;
            turn_end[effectIndex].card.cardViz.cardBorder.color = Color.blue;
            turn_end[effectIndex].Execute();
        }
        else if (turn_end[effectIndex].isDone && executing)
        {
            if (turn_end[effectIndex].card.EffectsDone())
            {
                GM.ActiveViz(turn_end[effectIndex].card);
            }

            executing = false;
            effectIndex++;
        }
    }

    bool EffectsAreDone()
    {
        for (int i = 0; i < turn_end.Count; i++)
        {
            if (!turn_end[i].isDone)
            {
                return false;
            }
        }

        return true;
    }

    public override bool IsComplete()
    {
        if ((turn_end.Count == 0) || (EffectsAreDone() && !executing))
        {
            if (GM.isMultiplayer)
            {
                /*
                if (!isDoneLocal)
                {
                    isDoneLocal = true;

                    GM.currentPlayer = GM.clientPlayer;
                    PhaseControllerChangeEvent.Raise();

                    int[] handCards = new int[GM.localPlayer.handCards.Count];
                    int[] deckCards = new int[GM.localPlayer.deck.Count];
                    int[] discardCards = new int[GM.localPlayer.discardCards.Count];
                    int[] playedCards = new int[GM.localPlayer.playedCards.Count];

                    for (int i = 0; i < handCards.Length; i++)
                    {
                        handCards[i] = GM.localPlayer.handCards[i].instanceId;
                    }

                    for (int i = 0; i < deckCards.Length; i++)
                    {
                        deckCards[i] = GM.localPlayer.deck[i].instanceId;
                    }

                    for (int i = 0; i < discardCards.Length; i++)
                    {
                        discardCards[i] = GM.localPlayer.discardCards[i].instanceId;
                    }

                    for (int i = 0; i < playedCards.Length; i++)
                    {
                        playedCards[i] = GM.localPlayer.playedCards[i].instanceId;
                    }

                    MultiplayerManager.singleton.SyncronizeAllCards(GM.localPlayer.photonId, handCards, deckCards, discardCards, playedCards);

                    return false;
                }
                else if (isDoneLocal && isDoneClient)
                {
                    MultiplayerManager.singleton.PlayerIsReady();
                }*/

                if (MultiplayerManager.singleton.ArePlayersReady())
                {
                    return CheckVictoryConditions();
                }
            }
            else
            {
                return true;
            }
        }
        else
        {
            ExecuteEffect();
        }

        return false;
    }

    public override void OnStartPhase()
    {
        base.OnStartPhase();

        if (!isInit)
        {
            turn_end.Clear();

            GM.currentPlayer = null;
            GM.SetState(null);
            GM.onPhaseChange.Raise();
            GM.onPhaseControllerChange.Raise();

            LoadCardEffects();

            PlayerHolder p = GM.localPlayer;

            for (int i = 0; i < p.playedCards.Count; i++)
            {
                if (!p.playedCards[i].cardEffects.Exists(b => b.type == EffectType.PREVAIL))
                {
                    p.discardCards.Add(p.playedCards[i]);
                }
            }

            for (int i = 0; i < p.handCards.Count; i++)
            {
                if (!p.handCards[i].cardEffects.Exists(b => b.type == EffectType.MAINTAIN))
                {
                    p.discardCards.Add(p.handCards[i]);
                }
            }

            p.handCards.RemoveAll(a => a.cardEffects.Exists(b => b.type != EffectType.MAINTAIN));
            p.playedCards.RemoveAll(a => a.cardEffects.Exists(b => b.type != EffectType.PREVAIL));

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
