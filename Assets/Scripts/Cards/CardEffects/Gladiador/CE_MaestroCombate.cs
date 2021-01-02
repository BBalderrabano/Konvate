using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Gladiador/Maestro del Combate")]
public class CE_MaestroCombate : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        Card copy = card.owner.handCards.Find(a => a.cardName == card.cardName);

        skipsEffectPreview = (copy == null);

        if (copy != null)
        {
            parentAction.PushAction(new A_ForcePlayCard(card.owner.photonId, copy.instanceId, parentAction.actionId));
        }
    }
}
