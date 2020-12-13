using UnityEngine;
using System.Collections;

public class A_SendToOpponentDeck : KAction
{
    readonly int cardTarget;
    readonly int sendToPosition;
    readonly int targetPhotonId;

    public A_SendToOpponentDeck(int cardId, int photonId, int targetPhotonId, int sendToPosition = -1, int actionId = -1, int cardOriginId = 0, int effectOriginId = 0) : base(photonId, actionId)
    {
        cardOrigin = cardOriginId;
        effectOrigin = effectOriginId;
        cardTarget = cardId;
        this.sendToPosition = sendToPosition;
        this.targetPhotonId = targetPhotonId;
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
            PlayerHolder target = GM.GetPlayerHolder(targetPhotonId);
            Card card = player.GetCard(cardTarget);

            foreach (CardEffect effect in card.cardEffects)
            {
                effect.isDone = false;

                if (effect is CE_HandCheck)
                {
                    ((CE_HandCheck)effect).linkedCardEffect.isDone = false;
                }
            }

            card.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.deckLogic);
            card.MakeBorderInactive();

            player.handCards.Remove(card);
            player.playedCards.Remove(card);

            if (sendToPosition < 0)
            {
                target.deck.Add(card);
            }
            else
            {
                target.deck.Insert(sendToPosition, card);
            }

            LinkAnimation(GM.animationManager.MoveCard(actionId, photonId, card.instanceId, target.currentHolder.deckGrid.value.position, target.currentHolder.deckGrid.value.gameObject));

            AudioManager.singleton.Play(SoundEffectType.SHUFFLE_DECK);

            isInit = true;

            GM.ChangeCardOwner(card.instanceId, photonId, targetPhotonId);
        }
    }

    public override bool IsComplete()
    {
        return isInit && AnimationsAreReady();
    }
}
