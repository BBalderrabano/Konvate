using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Nigromante/On Discard Place Chip")]
public class CE_OnDiscardPlaceChip : CardEffect
{
    public int amount = 1;
    public ChipType chip_type = ChipType.COMBAT;

    public override void Execute()
    {
        parentAction.MakeActiveOnComplete(false);

        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, chip_type, amount));
    }
}
