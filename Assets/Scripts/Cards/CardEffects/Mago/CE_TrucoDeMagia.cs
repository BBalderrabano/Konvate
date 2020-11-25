using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Card Effects/Mago/Truco de Magia")]
public class CE_TrucoDeMagia : SelectionCardEffect
{
    public override void Execute()
    {
        base.Execute();

        List<Card> hand_cards = card.owner.handCards;

        Card selected;

        if(hand_cards.Count > 0)
        {
            if(hand_cards.Count == 1)
            {
                selected = hand_cards.First();
                parentAction.PushAction(new A_SendToDeck(selected.instanceId, selected.owner.photonId, selected.owner.deck.Count));
            }
            else
            {
                if (card.owner.isLocal)
                {
                    parentAction.PushAction(new A_CardSelection("<b>Envia</b> una carta al fondo de tu mazo", hand_cards, card.owner.photonId, this, card.instanceId));
                }
                else
                {
                    GM.actionManager.PushAction(parentAction.actionId, new A_CardSelectionWait(GM.localPlayer.photonId, this, card.instanceId));
                }
            }
        }
        else
        {
            isDone = true;
        }
    }

    public override void DoneSelecting(int[] cardIds)
    {
        if (cardIds != null)
        {
            for (int i = 0; i < cardIds.Length; i++)
            {
                Card selected = card.owner.GetCard(cardIds[i]);
                parentAction.PushAction(new A_SendToDeck(selected.instanceId, selected.owner.photonId, selected.owner.deck.Count));
            }
        }

        isDone = true;
    }
}
