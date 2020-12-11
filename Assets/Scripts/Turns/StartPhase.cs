using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Turns/Phases/Start Phase")]
public class StartPhase : Phase
{
    List<CardEffect> turn_start = new List<CardEffect>();

    void LoadCardEffects()
    {
        int offensivePhotonId = GM.turn.offensivePlayer.photonId;

        for (int i = 0; i < GM.turn.startTurnEffects.Count; i++)
        {
            if (GM.turn.startTurnEffects[i].isDone)
                continue;

            if (GM.turn.startTurnEffects[i].type == EffectType.STARTTURN)
            {
                turn_start.Add(GM.turn.startTurnEffects[i]);
            }
        }

        turn_start = turn_start.OrderBy(a => a.priority).ThenBy(a => (a.card.owner.photonId != offensivePhotonId)).ToList();
    }

    void ExecuteEffects()
    {
        foreach (CardEffect eff in turn_start)
        {
            if (eff.isDone) { continue; }

            Action turn_start = new A_ExecuteEffect(eff.card.instanceId, eff.effectId, eff.card.owner.photonId);
            GM.actionManager.AddAction(turn_start);
        }
    }

    public override bool IsComplete()
    {
        return isInit && (turn_start.Count == 0 || GM.actionManager.IsDone());
    }

    public override bool CanPlayCard(Card c)
    {
        return false;
    }

    public override void OnStartPhase()
    {
        if (!isInit)
        {
            base.OnStartPhase();

            GM.SetState(null);
            GM.currentPlayer = null;

            GM.onPhaseChange.Raise();
            GM.onPhaseControllerChange.Raise();

            foreach (PlayerHolder p in GM.allPlayers)
            {
                p.statMods.RemoveAll(a => a.isTemporary);
            }

            foreach (Card c in GM.all_cards)
            {
                c.conditions.RemoveAll(a => a.isTemporary());
                c.cardEffects.RemoveAll(a => a.isTemporary);
                c.statMods.RemoveAll(a => a.isTemporary);
                c.cardViz.RefreshStats();
            }

            SetOffensivePlayer();

            LoadCardEffects();
            ExecuteEffects();

            MultiplayerManager.singleton.PhaseIsDone(GM.localPlayer.photonId, phaseIndex);

            isInit = true;
        }
    }

    void SetOffensivePlayer()
    {
        PlayerHolder currentOffPlayer = GM.turn.offensivePlayer;
        PlayerHolder opponent = GM.GetOpponentHolder(currentOffPlayer.photonId);

        Settings.SetParent(GM.turn.offensiveChip.transform, opponent.currentHolder.playedCombatChipHolder.value);

        GM.turn.offensiveChip.GetComponent<Chip>().owner = opponent;

        GM.turn.offensivePlayer = opponent;
    }

    public override void OnEndPhase()
    {
        if (isInit)
        {
            GM.SetState(null);
            isInit = false;

            for (int i = 0; i < GM.allPlayers.Length; i++)
            {
                GM.allPlayers[i].ResetMana();
                GM.allPlayers[i].clearFloatingDefend();
                GM.allPlayers[i].playerUI.UpdateAll();
            }

            turn_start.Clear();
            GM.turn.startTurnEffects.Clear();
        }
    }

    public override void OnTurnButtonHold()
    {
    }

    public override void OnTurnButtonPress()
    {
    }

    public override void OnPhaseControllerChange(int photonId)
    {
    }

    public override void OnPlayCard(Card c)
    {
    }
}
