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

        for (int i = 0; i < GM.turn.endTurnEffects.Count; i++)
        {
            if (GM.turn.endTurnEffects[i].isDone)
                continue;

            if (GM.turn.endTurnEffects[i].type == EffectType.ENDTURNSTART)
            {
                turn_start.Add(GM.turn.endTurnEffects[i]);
            }
        }

        turn_start = turn_start.OrderBy(a => (a.card.owner.photonId != offensivePhotonId)).ThenBy(a => a.priority).ToList();
    }

    public override bool IsComplete()
    {
        return isInit;
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

            turn_start.Clear();

            foreach (Card c in GameManager.singleton.all_cards)
            {
                c.conditions.RemoveAll(a => a.isTemporary());
                c.cardEffects.RemoveAll(a => a.isTemporary);
                c.cardViz.RefreshStats();
            }

            SetOffensivePlayer();

            for (int i = 0; i < GM.allPlayers.Length; i++)
            {
                GM.allPlayers[i].ResetMana();
                GM.allPlayers[i].clearFloatingDefend();
                GM.allPlayers[i].playerUI.UpdateAll();
            }

            LoadCardEffects();

            isInit = true;

            MultiplayerManager.singleton.PhaseIsDone(GM.localPlayer.photonId, phaseIndex);
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
            GM.turn.startTurnEffects.Clear();
            GM.SetState(null);
            isInit = false;
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
