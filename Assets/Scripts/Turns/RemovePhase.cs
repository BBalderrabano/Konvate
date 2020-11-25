using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Turns/Phases/Remove Phase")]
public class RemovePhase : Phase
{
    List<CardEffect> remove = new List<CardEffect>();

    void LoadCardEffects()
    {
        int offensivePhotonId = GM.turn.offensivePlayer.photonId;

        foreach (PlayerHolder player in GM.allPlayers)
        {
            foreach(Card c in player.playedCards)
            {
                if (c.isBroken)
                    continue;

                for (int i = 0; i < c.cardEffects.Count; i++)
                {
                    if (c.cardEffects[i].isDone)
                        continue;

                    if (c.cardEffects[i].type == EffectType.REMOVE)
                    {
                        remove.Add(c.cardEffects[i]);
                    }
                }
            }

            foreach(Card c in player.handCards)
            {
                foreach(CE_HandCheck hand_effect in c.cardEffects.OfType<CE_HandCheck>())
                {
                    if (hand_effect.linkedCardEffect.type == EffectType.REMOVE)
                    {
                        remove.Add(hand_effect.linkedCardEffect);
                    }
                }
            }
        }

        remove = remove.OrderBy(a => a.priority).ThenBy(a => (a.card.owner.photonId != offensivePhotonId)).ToList();            
    }

    void ExecuteEffects()
    {
        foreach (CardEffect eff in remove)
        {
            if (eff.isDone) { continue; }

            Action remove_effect = new A_ExecuteEffect(eff.card.instanceId, eff.effectId, eff.card.owner.photonId);
            GM.actionManager.AddAction(remove_effect);
        }
    }

    public override bool IsComplete()
    {
        return isInit && GM.actionManager.IsDone(); ;
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

            remove.Clear();

            LoadCardEffects();
            ExecuteEffects();

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
