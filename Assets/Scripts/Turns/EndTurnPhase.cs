﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Turns/Phases/End Turn Phase")]
public class EndTurnPhase : Phase
{
    List<CardEffect> endTurnStart = new List<CardEffect>();

    public SO.GameEvent PhaseControllerChangeEvent;

    void LoadCardEffects()
    {
        int offensivePhotonId = GM.turn.offensivePlayer.photonId;

        for (int i = 0; i < GM.turn.endTurnEffects.Count; i++)
        {
            if (GM.turn.endTurnEffects[i].card.isBroken)
                continue;

            if (GM.turn.endTurnEffects[i].isDone)
                continue;

            if (GM.turn.endTurnEffects[i].type == EffectType.ENDTURNSTART)
            {
                endTurnStart.Add(GM.turn.endTurnEffects[i]);
            }
        }

        endTurnStart = endTurnStart.OrderBy(a => a.priority).ThenBy(a => (a.card.owner.photonId != offensivePhotonId)).ToList();
    }

    void ExecuteEffect()
    {
        foreach (CardEffect eff in endTurnStart)
        {
            if (eff.isDone) { continue; }

            KAction end_turn_start = new A_ExecuteEffect(eff.card.instanceId, eff.effectId, eff.card.owner.photonId);
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
