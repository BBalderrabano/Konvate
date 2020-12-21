using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Goblin/Chantaje")]
public class CE_Chantaje : SelectionCardEffect
{
    public override void DoneSelecting(int[] cardIds)
    {
        if (cardIds != null)
        {
            Card selected = GM.GetCard(cardIds.First());

            selected.BreakeCard();
        }

        Finish();
    }

    public override void Execute()
    {
        base.Execute();

        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        List<Card> quickCards = opponent.playedCards.FindAll(a => a.GetCardType() is QuickPlay && !a.EffectsDone() && !a.isBroken);

        if(quickCards.Count == 1)
        {
            quickCards.First().BreakeCard();
        }
        else if(quickCards.Count > 1)
        {
            if (card.owner.isLocal)
            {
                parentAction.PushAction(new A_CardSelection("Anula el efecto de una <b>carta</b>", quickCards, card.owner.photonId, this, card.instanceId));
            }
            else
            {
                GM.actionManager.PushAction(parentAction.actionId, new A_CardSelectionWait(GM.localPlayer.photonId, this, card.instanceId));
            }
        }
    }
}
