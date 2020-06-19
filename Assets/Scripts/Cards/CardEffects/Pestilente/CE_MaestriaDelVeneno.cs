using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Pestilente/Maestria del Veneno")]
public class CE_MaestriaDelVeneno : CardEffect
{
    List<Card> savedHand = new List<Card>();

    public override void Execute()
    {
        base.Execute();

        savedHand.AddRange(card.owner.handCards);

        parentAction.PushActions(GM.DrawCard(card.owner, 1, card.instanceId, effectId, parentAction));
    }

    public override void Finish()
    {
        if (card.owner.isLocal)
        {
            List<Card> currentHand = card.owner.handCards;

            currentHand = currentHand.Except(savedHand).ToList();

            if (currentHand[0].HasTags(new CardTags[] { CardTags.PLACES_POISON_CHIP }))
            {
                Execute();
                parentAction.readyToRemove = false;
            }
            else
            {
                base.Finish();
            }
        }
        else
        {
            base.Finish();
        }
    }
}
