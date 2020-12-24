using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Samurai/Ya Estas Muerto")]
public class CE_YaEstasMuerto : CardEffect
{
    public int amount = 3;

    public override void Execute()
    {
        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.COMBAT, 3));
        parentAction.MakeActiveOnComplete(false);
    }
}
