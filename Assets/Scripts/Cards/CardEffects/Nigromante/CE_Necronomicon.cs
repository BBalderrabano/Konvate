﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Nigromante/Necronomicon Discard")]
public class CE_Necronomicon : SelectionCardEffect
{
    public override void Execute()
    {
        base.Execute();

        List<Card> hand_cards = card.owner.handCards;

        if (hand_cards.Count > 0)
        {
            if (card.owner.isLocal)
            {
                parentAction.PushAction(new A_CardSelection("<b>Descarta</b> una carta para <b>robar</b> otra", hand_cards, card.owner.photonId, this, card.instanceId));
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

            Card selected = card.owner.GetCard(cardIds.First());

            parentAction.PushAction(new A_Discard(selected.instanceId, selected.owner.photonId, true));
            parentAction.PushActions(GM.DrawCard(card.owner, 1));
        }

        Finish();
    }
}
