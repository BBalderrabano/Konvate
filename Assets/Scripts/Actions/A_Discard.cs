using UnityEngine;

public class A_Discard : Action
{
    int cardTarget;
    bool waitForAnimation;

    public A_Discard(int cardId, int photonId, int actionId = -1, int cardOriginId = 0, int effectOriginId = 0, bool waitForAnimation = false) : base(photonId, actionId)
    {
        cardOrigin = cardOriginId;
        effectOrigin = effectOriginId;
        cardTarget = cardId;

        this.waitForAnimation = waitForAnimation;
    }

    public override bool Continue()
    {
        return waitForAnimation;
    }

    public override void Execute(float t)
    {
        if (!isInit)
        {
            PlayerHolder player = GM.GetPlayerHolder(photonId);
            Card card = player.GetCard(cardTarget);

            foreach (CardEffect effect in card.cardEffects)
            {
                effect.isDone = false;

                if (effect is CE_HandCheck)
                {
                    ((CE_HandCheck)effect).linkedCardEffect.isDone = false;
                }
            }

            card.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.discardLogic);
            card.MakeBorderInactive();

            player.handCards.Remove(card);
            player.playedCards.Remove(card);
            player.discardCards.Add(card);

            LinkAnimation(GM.animationManager.MoveCard(actionId, photonId, card.instanceId, player.currentHolder.discardGrid.value.position, player.currentHolder.discardGrid.value.gameObject));

            AudioManager.singleton.Play(SoundEffectType.DISCARD_CARD);

            isInit = true;
        }
    }

    public override bool IsComplete()
    {
        return isInit && AnimationsAreReady();
    }
}
