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

    public override void Execute()
    {
        if (!isInit)
        {
            PlayerHolder player = GM.getPlayerHolder(photonId);
            Card card = player.GetCard(cardTarget);

            foreach (CardEffect effect in card.cardEffects)
            {
                effect.isDone = false;
            }

            CardInstance physInstance = card.cardPhysicalInst;

            physInstance.setCurrentLogic(GM.resourcesManager.dataHolder.discardLogic);
            physInstance.viz.cardBorder.color = Color.black;

            player.handCards.Remove(card);
            player.playedCards.Remove(card);
            player.discardCards.Add(card);

            LinkAnimation(GM.animationManager.MoveCard(actionId, photonId, card.instanceId, player.currentHolder.discardGrid.value.position, player.currentHolder.discardGrid.value.gameObject));

            isInit = true;
        }
    }

    public override bool IsComplete()
    {
        return isInit && AnimationsAreReady();
    }
}
