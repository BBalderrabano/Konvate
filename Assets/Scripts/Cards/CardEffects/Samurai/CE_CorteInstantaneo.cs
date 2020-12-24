using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Samurai/Corte Instantaneo")]
public class CE_CorteInstantaneo : SelectionCardEffect
{
    public int amount = 1;

    public override void Execute()
    {
        base.Execute();

        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.COMBAT, amount));

        if (!card.owner.playedCards.Exists(a => a.HasTags(CardTag.DEFENSA)))
        {
            List<Card> playableHandCards = new List<Card>();

            playableHandCards.AddRange(card.owner.handCards.FindAll(a => a.GetCardType() is NormalPlay && !a.HasTags(CardTag.DEFENSA)));

            if (playableHandCards.Count > 0)
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
        }
    }

    public override void DoneSelecting(int[] cardIds)
    {
        if (cardIds != null)
        {
            parentAction.PushAction(new A_ForcePlayCard(card.owner.photonId, cardIds.First()));
        }
        Finish();
    }
}
