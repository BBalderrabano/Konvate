using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Brujo/Leer los Huesos")]
public class CE_LeerLosHuesos : SelectionCardEffect
{
    public int top_deck_amount = 3;

    public override void Execute()
    {
        base.Execute();

        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);
        
        if(opponent.deck.Count > 3)
        {
            List<Card> top_deck_cards = opponent.deck.Take(top_deck_amount).ToList();

            if (card.owner.isLocal)
            {
                parentAction.PushAction(new A_CardSelection("Envia al <b>fondo</b> del mazo oponente", top_deck_cards, card.owner.photonId, this, card.instanceId).ModifyParameters(false, true, 0, 3));
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
        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        if (cardIds != null)
        {
            List<Card> selected = new List<Card>();

            for (int i = 0; i < cardIds.Length; i++)
            {
                Card selected_card = GM.GetOpponentHolder(card.owner.photonId).GetCard(cardIds[i]);
                selected.Add(selected_card);
                opponent.deck.Remove(selected_card);
            }

            opponent.deck.InsertRange(opponent.deck.Count, selected);
        }

        Finish();
    }
}
