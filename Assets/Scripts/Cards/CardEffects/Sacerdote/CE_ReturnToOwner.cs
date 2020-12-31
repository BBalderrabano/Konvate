using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Sacerdote/Return to Owner")]
public class CE_ReturnToOwner : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        Debug.Log("ENTRO");

        skipsEffectPreview = (card == null);
        parentAction.MakeActiveOnComplete((card == null));

        if (card != null)
        {
            card.owner.deck.Remove(card);
            card.owner.discardCards.Remove(card);
            card.owner.playedCards.Remove(card);
            card.owner.handCards.Remove(card);
            card.owner.all_cards.Remove(card);

            PlayerHolder original_owner = GM.GetPlayerHolder(card.photonId);

            card.owner = original_owner;

            original_owner.all_cards.Add(card);

            parentAction.PushAction(new A_Discard(card.instanceId, card.photonId));
        }
    }
}
