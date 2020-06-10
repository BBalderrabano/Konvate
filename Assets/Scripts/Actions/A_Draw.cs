
using System.Linq;
using UnityEngine;

public class A_Draw : Action
{
    bool noCardsToDraw;

    public A_Draw(int photonId, int actionId = -1, int cardId = 0, int effectId = 0) : base(photonId, actionId) 
    {
        noCardsToDraw = false;
        cardOrigin = cardId;
        effectOrigin = effectId;
    }

    public override bool Continue()
    {
        return false;
    }

    public override void Execute(float t)
    {
        if (!isInit)
        {
            PlayerHolder player = GM.getPlayerHolder(photonId);

            if (player.deck.Count > 0)
            {
                Card card = player.deck[0];

                if (player.isLocal)
                {
                    card.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.handLogic);
                }
                else
                {
                    card.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.opponentHandLogic);
                }

                player.handCards.Add(card);
                player.deck.Remove(card);

                LinkAnimation(GM.animationManager.MoveCard(actionId, photonId, card.instanceId, player.currentHolder.handGrid.value.position, player.currentHolder.handGrid.value.gameObject));

                if (GM.isMultiplayer && player.isLocal)
                {
                    MultiplayerManager.singleton.SendDraw(photonId, actionId, cardOrigin, effectOrigin);
                }
            }
            else if (player.deck.Count == 0 && player.discardCards.Count > 0)
            {
                if (player.isLocal)
                {
                    Action shuffle = new A_Shuffle(photonId);

                    PushAction(shuffle);
                }
            }
            else if (player.deck.Count == 0 && player.discardCards.Count == 0)
            {
                noCardsToDraw = true;
            }

            isInit = true;
        }

        ExecuteLinkedAction(t);
    }

    public override bool IsComplete()
    {
        if (noCardsToDraw)
            return true;

        if (GM.isMultiplayer)
        {
            if (linkedActions.Any())
            {
                return AnimationsAreReady() && LinkedActionsReady();
            }
            else
            {
                return PlayersAreReady() && AnimationsAreReady();
            }
        }
        else
        {
            return AnimationsAreReady() && LinkedActionsReady();
        }
    }
}
