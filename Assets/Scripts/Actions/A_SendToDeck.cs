using UnityEngine;
using System.Collections;

public class A_SendToDeck : KAction
{
    int cardTarget;
    int sendToPosition;

    public A_SendToDeck(int cardId, int photonId, int sendToPosition = -1, int actionId = -1, int cardOriginId = 0, int effectOriginId = 0) : base(photonId, actionId)
    {
        cardOrigin = cardOriginId;
        effectOrigin = effectOriginId;
        cardTarget = cardId;
        this.sendToPosition = sendToPosition;
    }

    public override bool Continue()
    {
        return false;
    }

    public override void Execute(float t)
    {
        if (!isInit)
        {
            PlayerHolder player = GM.GetPlayerHolder(photonId);
            Card card = player.GetCard(cardTarget);

            card.ResetCardEffects();

            card.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.deckLogic);
            card.MakeBorderInactive();

            player.handCards.Remove(card);
            player.discardCards.Remove(card);
            player.playedCards.Remove(card);

            if(sendToPosition < 0)
            {
                player.deck.Add(card);
            }
            else
            {
                player.deck.Insert(sendToPosition, card);
            }

            LinkAnimation(GM.animationManager.MoveCard(actionId, photonId, card.instanceId, player.currentHolder.deckGrid.value.position, player.currentHolder.deckGrid.value.gameObject));

            AudioManager.singleton.Play(SoundEffectType.SHUFFLE_DECK);

            isInit = true;
        }
    }

    public override bool IsComplete()
    {
        return isInit && AnimationsAreReady();
    }
}
