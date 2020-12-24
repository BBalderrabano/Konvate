using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_ForcePlayCard : KAction
{
    float time = 0;
    readonly PlayerHolder player;
    readonly Card card;

    bool cardPlaced = false;

    public A_ForcePlayCard(int photonId, int cardId, int actionId = -1) : base(photonId, actionId)
    {
        player = GM.GetPlayerHolder(photonId);
        card = player.GetCard(cardId);
    }

    public override bool Continue()
    {
        return false;
    }

    public override void Execute(float t)
    {
        if (!isInit)
        {
            LinkAnimation(GM.animationManager.MoveCard(actionId, player.photonId, card.instanceId, player.currentHolder.playedGrid.value.position, player.currentHolder.playedGrid.value.gameObject, SoundEffectType.PLACE_CARD));
            time = 0;
            isInit = true;
            cardPlaced = false;

            card.RevealCard();

            foreach (CardEffect eff in card.cardEffects)
            {
                if (eff.type == EffectType.ON_PLAY_START)
                {
                    eff.Execute();
                }
            }
        }

        time += t;
    }

    public override bool IsComplete()
    {
        if(!cardPlaced && isInit && AnimationsAreReady())
        {
            player.handCards.Remove(card);
            player.deck.Remove(card);
            player.discardCards.Remove(card);

            player.playedCards.Add(card);

            foreach (CardEffect eff in card.cardEffects)
            {
                if (eff.type == EffectType.ON_PLAY_END)
                {
                    eff.Execute();
                }
            }

            GM.turn.currentPhase.value.OnPlayCard(card);

            cardPlaced = true;
        }

        return cardPlaced && isInit && AnimationsAreReady() && (time > Settings.CARD_EFFECT_MIN_PREVIEW);
    }
}
