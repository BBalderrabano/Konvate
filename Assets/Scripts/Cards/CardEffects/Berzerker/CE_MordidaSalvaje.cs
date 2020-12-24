using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Berzerker/Mordida Salvaje Place")]
public class CE_MordidaSalvaje : CardEffect
{
    public int combat_chip_amount = 1;
    public int bleed_chip_amount = 1;

    public override void Execute()
    {
        base.Execute();

        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.COMBAT, combat_chip_amount));

        if (IsCombo())
        {
            parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.BLEED, bleed_chip_amount));
        }
    }
}
