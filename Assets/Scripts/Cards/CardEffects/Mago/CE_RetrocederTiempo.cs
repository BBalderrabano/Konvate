using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Card Effects/Mago/Retroceder el Tiempo")]
public class CE_RetrocederTiempo : SelectionCardEffect
{
    public override void Execute()
    {
        base.Execute();

        List<Card> discard_cards = card.owner.discardCards;

        if (discard_cards.Count > 0)
        {
            if (card.owner.isLocal)
            {
                parentAction.PushAction(new A_CardSelection("<b>Envia</b> hasta 5 cartas al fondo de tu mazo", discard_cards, card.owner.photonId, this, card.instanceId).ModifyParameters(true, 0, 5));
            }
            else
            {
                GM.actionManager.PushAction(parentAction.actionId, new A_CardSelectionWait(GM.localPlayer.photonId, this, card.instanceId));
            }
        }
        else
        {
            Finish();
        }
    }

    public override void DoneSelecting(int[] cardIds)
    {
        if (cardIds != null && cardIds.Length > 0)
        {
            for (int i = 0; i < cardIds.Length; i++)
            {
                Card selected = card.owner.GetCard(cardIds[i]);
                parentAction.PushAction(new A_SendToDeck(selected.instanceId, selected.owner.photonId, selected.owner.deck.Count));
            }
        }

        Finish();
    }
}
