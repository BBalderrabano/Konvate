using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CE_MaldicionPlacePoison : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        if (card.owner.photonId != card.photonId)
        {
            parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.photonId, card.instanceId, ChipType.POISON, 1));
        }
    }
}
