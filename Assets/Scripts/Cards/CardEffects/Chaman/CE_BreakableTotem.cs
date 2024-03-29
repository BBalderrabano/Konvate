﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Chaman/Totem Destruible")]
public class CE_BreakableTotem : SelectionCardEffect
{
    public int amount_required = 1;
    public CardEffect totem_effect;
    public Card stone_totem;
    bool breakeIt = false;

    public override void Execute()
    {
        base.Execute();

        breakeIt = false;

        PlayerHolder enemy = GM.GetOpponentHolder(card.owner.photonId);
        int played_combat_chips = enemy.currentHolder.playedCombatChipHolder.value.childCount;

        if (card.owner.playedCards.Find(a => a.cardName == stone_totem.cardName) &&
            !card.owner.playedCards.Find(a => a.cardName == stone_totem.cardName).isBroken)
        {
            ExecuteTotemEffect();
        }
        else
        {
            if (played_combat_chips >= amount_required)
            {
                if (card.owner.isLocal)
                {
                    GM.actionManager.PushAction(parentAction.actionId, new A_CardSelectionWait(GM.localPlayer.photonId, this, card.instanceId, enemy.photonId));
                }
                else
                {
                    parentAction.PushAction(new A_CardSelection("¿Usar " + amount_required + " <sprite=0>?\nTienes: " + played_combat_chips, card, enemy.photonId, this, card.instanceId).ModifyParameters(true, false, 0, 0));
                }
            }
            else
            {
                ExecuteTotemEffect();
            }
        }
    }

    public override void DoneSelecting(int[] cardIds)
    {
        if (cardIds == null)
        {
            ExecuteTotemEffect();
            breakeIt = false;
        }
        else
        {
            parentAction.LinkAnimation(GM.animationManager.RemoveChip(parentAction.actionId, this, card.owner.photonId, card.instanceId, ChipType.COMBAT_OFFENSIVE, amount_required));
            breakeIt = true;
        }
    }

    void ExecuteTotemEffect()
    {
        CardEffect clone = totem_effect.Clone(card);

        card.cardEffects.Add(clone);

        parentAction.PushAction(new A_ExecuteEffect(card.instanceId, clone.effectId, card.owner.photonId));
    }

    public override void Finish()
    {
        if (breakeIt)
        {
            card.BreakeCard();
        }
        else
        {
            card.MakeBorderActive();
        }

        base.Finish();
    }
}
