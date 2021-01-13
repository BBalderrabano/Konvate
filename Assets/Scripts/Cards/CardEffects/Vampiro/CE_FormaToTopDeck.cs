using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Vampiro/Forma to top deck")]
public class CE_FormaToTopDeck : SelectionCardEffect
{
    public override void Execute()
    {
        base.Execute();

        parentAction.MakeActiveOnComplete(true);

        if (card.owner.isLocal)
        {
            parentAction.PushAction(new A_CardSelection("¿Enviar al <b>tope</b> del mazo?", card, card.owner.photonId, this, card.instanceId).ModifyParameters(true, false, 0, 0));
        }
        else
        {
            GM.actionManager.PushAction(parentAction.actionId, new A_CardSelectionWait(GM.localPlayer.photonId, this, card.instanceId));
        }
    }

    public override void DoneSelecting(int[] cardIds)
    {
        if (cardIds != null)
        {
            parentAction.MakeActiveOnComplete(false);
            parentAction.PushAction(new A_SendToDeck(card.instanceId, card.owner.photonId, 0));
        }

        Finish();
    }
}
