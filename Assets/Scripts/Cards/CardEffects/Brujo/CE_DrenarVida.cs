using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Brujo/Drenar Vida")]
public class CE_DrenarVida : CardEffect
{
    readonly List<Card> savedHand = new List<Card>();
    PlayerHolder opponent;

    public override void Execute()
    {
        base.Execute();

        savedHand.Clear();

        opponent = GM.GetOpponentHolder(card.owner.photonId);

        savedHand.AddRange(opponent.handCards);

        parentAction.PushActions(GM.DrawCard(opponent, 1, card.instanceId, effectId, parentAction));
    }

    public override void Finish()
    {
        List<Card> currentHand = card.owner.handCards;

        currentHand = currentHand.Except(savedHand).ToList();

        Card cardDrawn = currentHand[0];

        cardDrawn.RevealCard();

        GM.PreviewCard(cardDrawn);

        if (cardDrawn.HasTags(new CardTags[] { CardTags.CURSE_BRUJO }))
        {
            parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.COMBAT, 1));
            parentAction.readyToRemove = false;
        }
        else
        {
            base.Finish();
        }
    }
}
