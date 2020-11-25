using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Card Effects/Mago/Congelar")]
public class CE_Congelar : SelectionCardEffect
{
    public override void Execute()
    {
        base.Execute();

        List<Card> played_cards = GM.GetOpponentHolder(card.owner.photonId).playedCards;

        List<Card> played_normal_cards = new List<Card>();

        played_normal_cards.AddRange(played_cards.Where(a => a.GetCardType() is NormalPlay && a.isBroken == false));

        if (played_normal_cards.Count > 0)
        {
            if (played_normal_cards.Count == 1)
            {
                Card selected = played_normal_cards.First();
                selected.BreakeCard();
                Finish();
            }
            else
            {
                if (card.owner.isLocal)
                {
                    parentAction.PushAction(new A_CardSelection("<b>Anula</b> una carta oponente", played_normal_cards, card.owner.photonId, this, card.instanceId));
                }
                else
                {
                    GM.actionManager.PushAction(parentAction.actionId, new A_CardSelectionWait(GM.localPlayer.photonId, this, card.instanceId));
                }
            }
        }
        else
        {
            Finish();
        }
    }

    public override void DoneSelecting(int[] cardIds)
    {
        if (cardIds != null)
        {
            for (int i = 0; i < cardIds.Length; i++)
            {
                Card selected = GM.GetOpponentHolder(card.owner.photonId).GetCard(cardIds[i]);
                selected.BreakeCard();
            }
        }

        Finish();
    }
}
