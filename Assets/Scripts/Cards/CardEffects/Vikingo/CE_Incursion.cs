using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Vikingo/Incursion")]
public class CE_Incursion : SelectionCardEffect
{
    public override void Execute()
    {
        List<Card> available = new List<Card>();

        available.AddRange(card.owner.deck);
        available.AddRange(card.owner.discardCards);

        if(available.Count > 0)
        {
            if(available.Count == 1)
            {
                parentAction.PushAction(new A_DrawSpecific(card.owner.photonId, available.First().instanceId));
            }
            else
            {
                List<Card> curated = new List<Card>();

                for (int i = 0; i < available.Count; i++)
                {
                    if (!curated.Exists(a => a.cardName == available[i].cardName))
                    {
                        curated.Add(available[i]);
                    }
                }

                if (card.owner.isLocal)
                {
                    parentAction.PushAction(new A_CardSelection("<b>Agrega</b> una carta a tu mano", curated, card.owner.photonId, this, card.instanceId));
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
            parentAction.PushAction(new A_DrawSpecific(card.owner.photonId, cardIds.First()));
        }

        Finish();
    }
}
