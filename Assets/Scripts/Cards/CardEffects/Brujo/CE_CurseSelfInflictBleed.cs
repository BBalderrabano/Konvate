using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Brujo/Curse self inflict bleed")]
public class CE_CurseSelfInflictBleed : CardEffect
{
    public override void Execute()
    {
        if (card.owner.photonId != card.photonId)
        {
            base.Execute();

            skipsEffectPreview = false;
            card.RevealCard();
            parentAction.LinkAnimation(GM.animationManager.DirectDamageBleedChip(parentAction.actionId, card.owner.photonId, card.instanceId, card.owner.photonId));
        }
        else
        {
            skipsEffectPreview = true;

            card.MakeBorderInactive();
            GM.HidePreviewCard();
        }
    }
}
