using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Duelista/Ajustar Estrategia")]
public class CE_AjustarEstrategia : SelectionCardEffect
{
    public override void Execute()
    {
        base.Execute();

        List<Card> hand_cards = card.owner.handCards;

        if (hand_cards.Count > 0)
        {
            if (card.owner.isLocal)
            {
                parentAction.PushAction(new A_CardSelection("<b>Descarta</b> cartas y roba la misma cantidad", hand_cards, card.owner.photonId, this, card.instanceId).ModifyParameters(false, true, 0, hand_cards.Count));
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
        List<KAction> discard = new List<KAction>();

        if (cardIds != null)
        {
            for (int i = 0; i < cardIds.Length; i++)
            {
                Card selected = card.owner.GetCard(cardIds[i]);

                discard.Add(new A_Discard(selected.instanceId, selected.owner.photonId, true));
            }

            parentAction.PushActions(discard);
            GM.DrawCard(card.owner, cardIds.Length);
        }

        Finish();
    }
}
