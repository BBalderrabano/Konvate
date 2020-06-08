using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Turns/Phases/Start Phase")]
public class StartPhase : Phase
{
    List<CardEffect> turn_start = new List<CardEffect>();

    void LoadCardEffects()
    {
        ///
        /// TODO: Aqui tiene que loopear una lista "start turn effects" que tiene effectholders para poder poner la carta en el campo, mostrarla, hacer el efecto y regresarla al mazo/descarte/mano sin que se vea
        //////
        /*int offensivePhotonId = gm.turn.offensivePlayer.photonId;

        foreach (PlayerHolder player in gm.allPlayers)
        {
            foreach (Card c in player.playedCards)
            {
                for (int i = 0; i < c.cardEffects.Count; i++)
                {
                    if (c.cardEffects[i].isDone)
                        continue;

                    if (c.cardEffects[i].type == EffectType.RESTORE)
                    {
                        turn_start.Add(c.cardEffects[i]);
                    }
                }
            }
        }
        
                 turn_start = turn_start.OrderBy(a => a.priority).ToList();
        */
    }

    /*
    bool executing = false;
    int effectIndex = 0;

    void ExecuteEffect()
    {
        if (!turn_start[effectIndex].isDone && !executing)
        {
            executing = true;
            turn_start[effectIndex].card.cardViz.cardBorder.color = Color.blue;
            turn_start[effectIndex].Execute();
        }
        else if (turn_start[effectIndex].isDone && executing)
        {
            if (turn_start[effectIndex].card.EffectsDone())
            {
                GM.ActiveViz(turn_start[effectIndex].card);
            }

            executing = false;
            effectIndex++;
        }
    }

    bool EffectsAreDone()
    {
        for (int i = 0; i < turn_start.Count; i++)
        {
            if (!turn_start[i].isDone)
            {
                return false;
            }
        }

        return true;
    }*/

    public override bool IsComplete()
    {
        /*if ((turn_start.Count == 0) || (EffectsAreDone() && !executing))
        {
            if (GM.isMultiplayer)
            {
                if (!isDoneLocal)
                {
                    MultiplayerManager.singleton.PhaseIsDone(GM.localPlayer.photonId, this.phaseIndex);

                    isDoneLocal = true;

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

        return isInit && PlayersAreReady();
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
            }

            LoadCardEffects();

            isInit = true;

            MultiplayerManager.singleton.PhaseIsDone(GM.localPlayer.photonId, phaseIndex);
        }
    }

    void SetOffensivePlayer()
    {
        PlayerHolder currentOffPlayer = GM.turn.offensivePlayer;
        PlayerHolder opponent = GM.getOpponentHolder(currentOffPlayer.photonId);

        GM.turn.offensiveChip.transform.SetParent(opponent.currentHolder.playedCombatChipHolder.value);

        GM.turn.offensiveChip.transform.position = Vector3.zero;
        GM.turn.offensiveChip.transform.rotation = Quaternion.identity;
        GM.turn.offensiveChip.transform.localScale = Vector3.one;

        GM.turn.offensivePlayer = opponent;
    }

    public override void OnEndPhase()
    {
        if (isInit)
        {
            GameManager.singleton.SetState(null);

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
