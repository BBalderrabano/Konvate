using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Berzerker/Ira Berzerker")]
public class CE_IraBerzerker : SelectionCardEffect
{
    public int amount = 1;

    public override void Execute()
    {
        base.Execute();

        skipsEffectPreview = false;
        parentAction.MakeActiveOnComplete(true);

        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.COMBAT, amount));

        if (card.owner.playedCards.Exists(a => a.HasTags(CardTag.ATAQUE_BASICO)))
        {
            List<Card> playableHandCards = new List<Card>();

            playableHandCards.AddRange(card.owner.handCards.FindAll(a => a.GetCardType() is NormalPlay && a.photonId == card.owner.photonId));
            
            if(playableHandCards.Count > 0)
            {
                if (card.owner.isLocal)
                {
                    parentAction.PushAction(new A_CardSelection("Pon en <b>juego</b> una carta", playableHandCards, card.owner.photonId, this, card.instanceId));
                }
                else
                {
                    GM.actionManager.PushAction(parentAction.actionId, new A_CardSelectionWait(GM.localPlayer.photonId, this, card.instanceId));
                }
            }
            else
            {
                skipsEffectPreview = true;
            }
        }
        else
        {
            skipsEffectPreview = true;
        }
    }

    public override void DoneSelecting(int[] cardIds)
    {
        if(cardIds != null)
        {
            parentAction.MakeActiveOnComplete(false);
            parentAction.PushAction(new A_ForcePlayCard(card.owner.photonId, cardIds.First()));
            parentAction.PushAction(new A_SendToDeck(card.instanceId, card.owner.photonId, 0));
        }
    }
}
