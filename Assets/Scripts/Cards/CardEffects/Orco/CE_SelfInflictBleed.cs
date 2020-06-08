using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Self inflict bleed")]
public class CE_SelfInflictBleed : CardEffect
{
    public override void Execute()
    {
        //GameManager.singleton.CreateBleedChipToDamagePlayer(card.owner.photonId, card.owner.photonId, card.instanceId, this, 1, true);
        Finish();
        //parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.BLEEd, 1));
    }
}
