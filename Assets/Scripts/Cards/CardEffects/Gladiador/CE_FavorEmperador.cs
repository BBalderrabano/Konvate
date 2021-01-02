using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Gladiador/Favor del Emperador")]
public class CE_FavorEmperador : SelectionCardEffect
{
    public override void Execute()
    {
        base.Execute();

        List<Card> available = card.owner.discardCards;

        if(available.Count > 0)
        {
            if (card.owner.isLocal)
            {
                parentAction.PushAction(new A_CardSelection("<b>Coloca</b> una carta en tu mano", available, card.owner.photonId, this, card.instanceId));
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
        if (cardIds != null)
        {
            parentAction.PushAction(new A_DrawSpecific(card.owner.photonId, cardIds.First()));
        }

        Finish();
    }
}
