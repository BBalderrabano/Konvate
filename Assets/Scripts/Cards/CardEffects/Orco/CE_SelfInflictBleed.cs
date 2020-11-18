using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Orco/Self inflict bleed")]
public class CE_SelfInflictBleed : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        parentAction.LinkAnimation(GM.animationManager.DirectDamageBleedChip(parentAction.actionId, card.owner.photonId, card.instanceId, card.owner.photonId));
    }
}
