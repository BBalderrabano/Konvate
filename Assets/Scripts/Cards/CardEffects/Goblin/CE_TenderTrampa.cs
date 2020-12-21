using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Goblin/Tender Trampa")]
public class CE_TenderTrampa : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        List<Card> cardsToSend = new List<Card>();

        cardsToSend.AddRange(card.owner.discardCards);
        cardsToSend.AddRange(card.owner.handCards);
        cardsToSend.AddRange(card.owner.playedCards);

        List<KAction> sendToDeck = new List<KAction>();

        foreach (Card c in cardsToSend)
        {
            sendToDeck.Add(new A_SendToDeck(c.instanceId, c.owner.photonId));
        }

        parentAction.PushActions(sendToDeck);

        parentAction.MakeActiveOnComplete(false);
    }

    public override void Finish()
    {
        parentAction.PushAction(new A_Shuffle(card.owner.photonId));

        base.Finish();
    }
}
