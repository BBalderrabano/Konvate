using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Self inflict bleed")]
public class CE_SelfInflictBleed : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, GM.GetOpponentHolder(card.owner.photonId).photonId, card.instanceId, ChipType.BLEED, 1));
    }
}
