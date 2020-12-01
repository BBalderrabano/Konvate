using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Enano/Martillo")]
public class CE_Martillo : SelectionCardEffect
{
    public override void Execute()
    {
        base.Execute();

        List<Card> hand_cards = card.owner.handCards;

        if (hand_cards.Count > 0)
        {
            if (card.owner.isLocal)
            {
                parentAction.PushAction(new A_CardSelection("<b>Descarta</b> y cada 2 <color=#FF0000>coloca </color>1<sprite=0>", hand_cards, card.owner.photonId, this, card.instanceId).ModifyParameters(true, 0, 999));
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
            int amount = cardIds.Length / 2;

            parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.COMBAT, amount));
        }
    }
}
