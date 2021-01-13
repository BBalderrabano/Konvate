using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Vampiro/Forma Murcielago")]
public class CE_FormaMurcielago : CardEffect
{
    public Card targetCard;

    public override void Execute()
    {
        base.Execute();

        if(card.owner.discardCards.Exists(a => a.cardName == targetCard.cardName))
        {
            Card c = card.owner.discardCards.Find(a => a.cardName == targetCard.cardName);

            parentAction.PushAction(new A_DrawSpecific(card.owner.photonId, c.instanceId, parentAction.actionId, card.instanceId, effectId));
        }
        else
        {
            Finish();
        }
    }
}
