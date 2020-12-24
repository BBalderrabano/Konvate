using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Berzerker/Bravado Combo")]
public class CE_BravadoCombo : CardEffect
{
    public int amount = 2;

    public override void Execute()
    {
        base.Execute();

        skipsEffectPreview = false;

        if (IsCombo())
        {
            parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.COMBAT, amount));
        }
        else
        {
            skipsEffectPreview = true;
        }
    }
}
