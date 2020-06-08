using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Turns/Phases/End Turn Phase")]
public class EndTurnPhase : Phase
{
    int effectIndex = 0;
    List<CardEffect> endTurnStart = new List<CardEffect>();

    public SO.GameEvent PhaseControllerChangeEvent;

    void LoadCardEffects()
    {
        if (GM.isMultiplayer)
        {
            int offensivePhotonId = GM.turn.offensivePlayer.photonId;

            foreach (PlayerHolder player in GM.allPlayers)
            {
                foreach (Card c in player.playedCards)
                {
                    for (int i = 0; i < c.cardEffects.Count; i++)
                    {
                        if (c.cardEffects[i].isDone)
                            continue;

                        if (c.cardEffects[i].type == EffectType.ENDTURNSTART)
                        {
                            endTurnStart.Add(c.cardEffects[i]);
                        }
                    }
                }
            }

            endTurnStart = endTurnStart.OrderBy(a => (a.card.owner.photonId != offensivePhotonId)).ThenBy(a => a.priority).ToList();
        }
    }

    bool executing = false;

    void ExecuteEffect()
    {
        if (!effectsAreDone())
        {
            if (!endTurnStart[effectIndex].isDone && !executing)
            {
                executing = true;
                endTurnStart[effectIndex].card.cardViz.cardBorder.color = Color.blue;
                endTurnStart[effectIndex].Execute();
            }
            else if (endTurnStart[effectIndex].isDone && executing)
            {
                if (endTurnStart[effectIndex].card.EffectsDone())
                {
                    GM.ActiveViz(endTurnStart[effectIndex].card);
                }
                executing = false;
                effectIndex++;
            }
        }
    }

    bool effectsAreDone()
    {
        for (int i = 0; i < endTurnStart.Count; i++)
        {
            if (!endTurnStart[i].isDone)
            {
                return false;
            }
        }

        return true;
    }

    public override bool IsComplete()
    {
        if (!isInit)
        {
            return false;
        }
        /*
        if ((endTurnStart.Count == 0) || (effectsAreDone() && !executing))
        {
            if (gm.isMultiplayer)
            {
                if (!isDoneLocal)
                {
                    MultiplayerManager.singleton.PhaseIsDone(gm.localPlayer.photonId, this.phaseIndex);

                    isDoneLocal = true;

                    gm.currentPlayer = gm.clientPlayer;
                    PhaseControllerChangeEvent.Raise();

                    return false;
                }
                else if (isDoneClient && isDoneLocal)
                {
                    return true;
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
        }*/

        return false;
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

            endTurnStart.Clear();
            effectIndex = 0;
            executing = false;

            LoadCardEffects();

            MultiplayerManager.singleton.PlayerIsNotReady();

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

    public override void OnTurnButtonPress()
    {
    }
    public override void OnTurnButtonHold()
    {
    }

    public override void OnPhaseControllerChange(int photonId)
    {
    }

    public override void OnPlayCard(Card c)
    {
    }
}
