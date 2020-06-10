﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Turns/Phases/End Turn Phase")]
public class EndTurnPhase : Phase
{
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

    void ExecuteEffect()
    {
        foreach (CardEffect eff in endTurnStart)
        {
            if (eff.isDone) { continue; }

            Action end_turn_start = new A_ExecuteEffect(eff.card.instanceId, eff.effectId, eff.card.owner.photonId);
            GM.actionManager.AddAction(end_turn_start);
        }
    }

    public override bool IsComplete()
    {
        return isInit && GM.actionManager.IsDone();
    }

    public override void OnStartPhase()
    {
        base.OnStartPhase();

        if (!isInit)
        {
            MultiplayerManager.singleton.PlayerIsNotReady();

            GM.currentPlayer = null;
            GM.SetState(null);
            GM.onPhaseChange.Raise();
            GM.onPhaseControllerChange.Raise();

            endTurnStart.Clear();

            LoadCardEffects();

            ExecuteEffect();

            isInit = true;
        }
    }

    public override void OnEndPhase()
    {
        if (isInit)
        {
            GM.SetState(null);
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
