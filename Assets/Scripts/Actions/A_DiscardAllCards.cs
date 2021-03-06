﻿using UnityEngine;
using System.Collections.Generic;

public class A_DiscardAllCards : KAction
{
    public A_DiscardAllCards(int photonId, int actionId = -1) : base(photonId, actionId) {}

    public override bool Continue()
    {
        return false;
    }

    public override void Execute(float t)
    {
        if (!isInit)
        {
            List<KAction> discardActions = new List<KAction>();
            List<Card> cardsToDiscard = new List<Card>();

            PlayerHolder player = GM.GetPlayerHolder(photonId);

            cardsToDiscard.AddRange(PrepareForDiscard(player.handCards));
            cardsToDiscard.AddRange(PrepareForDiscard(player.playedCards));

            for (int i = 0; i < cardsToDiscard.Count; i++)
            {
                Card card = cardsToDiscard[i];

                card.FixCard();

                A_Discard discard_card = new A_Discard(card.instanceId, photonId);

                discardActions.Add(discard_card);
            }

            PushActions(discardActions);

            isInit = true;
        }

        ExecuteLinkedAction(t);
    }

    List<Card> PrepareForDiscard(List<Card> cardList)
    {
        List<Card> cardsToDiscard = new List<Card>();

        for (int i = 0; i < cardList.Count; i++)
        {
            bool skip = false;

            Card card = cardList[i];
            CardInstance physInstance = card.cardPhysicalInst;

            foreach (CardEffect effect in card.cardEffects)
            {
                effect.Reset();

                if(effect is CE_HandCheck)
                {
                    ((CE_HandCheck)effect).linkedCardEffect.Reset();
                }

                if ((effect.type == EffectType.MAINTAIN && card.owner.handCards.Contains(card)) || (effect.type == EffectType.PREVAIL && card.owner.playedCards.Contains(card)))
                {
                    card.RevealCard();
                    skip = true;
                }
            }

            if (!skip)
            {
                physInstance.setCurrentLogic(GM.resourcesManager.dataHolder.discardLogic);

                card.MakeBorderInactive();

                cardsToDiscard.Add(card);
            }
        }

        return cardsToDiscard;
    }

    public override bool IsComplete()
    {
        return isInit && LinkedActionsReady();
    }
}
