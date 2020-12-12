using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Place Chip")]
public class CE_PlaceChip : CardEffect
{
    public ChipType chipType = ChipType.COMBAT;
    public int amount = 1;
    public target_type placed_by = target_type.CARD_OWNER;

    public override void Execute()
    {
        base.Execute();

        PlayerHolder playerTarget;

        if (placed_by == target_type.CARD_OWNER)
        {
            playerTarget = card.owner;
        }
        else
        {
            playerTarget = GM.GetOpponentHolder(card.owner.photonId);
        }

        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, playerTarget.photonId, card.instanceId, chipType, amount));
    }

    public enum target_type
    {
        CARD_OWNER,
        OPPONENT
    }
}