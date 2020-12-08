using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/On Recharge Discard Shape")]
public class CE_OnRechargeDiscardShape : CardEffect
{
    public override void Execute()
    {
        parentAction.PushAction(new A_Discard(card.instanceId, card.owner.photonId));

        skipsEffectPreview = true;
    }
}
