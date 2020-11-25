using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Card Effects/Orco/Porrazo")]
public class CE_Porrazo : SelectionCardEffect
{
    public override void Execute()
    {
        base.Execute();

        List<Card> played_cards = GM.GetOpponentHolder(card.owner.photonId).playedCards;

        List<Card> defense = new List<Card>();

        defense.AddRange(played_cards.Where(a => a.HasTags(CardTag.DEFENSA)).ToList());
        defense.AddRange(played_cards.Where(a => a.HasTags(CardTag.DEFENSA_SUPERIOR)).ToList());

        if(defense.Count > 0)
        {
            if(defense.Count == 1)
            {
                Card card = defense.First();
                parentAction.PushAction(new A_Discard(card.instanceId, card.owner.photonId));
            }
            else
            {
                if (card.owner.isLocal)
                {
                    parentAction.PushAction(new A_CardSelection("<b>Destruye</b> una defensa o defensa superior", defense, card.owner.photonId, this, card.instanceId));
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
                parentAction.PushAction(new A_Discard(selected.instanceId, selected.owner.photonId));
            }
        }

        Finish();
    }
}
