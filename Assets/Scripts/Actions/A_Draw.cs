using System.Linq;

public class A_Draw : KAction
{
    Card cardDrawn = null;
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
            PlayerHolder player = GM.GetPlayerHolder(photonId);

            if (player.deck.Count > 0)
            {
                cardDrawn = player.deck[0];

                if (player.isLocal)
                {
                    cardDrawn.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.handLogic);
                }
                else
                {
                    cardDrawn.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.opponentHandLogic);
                }

                player.handCards.Add(cardDrawn);
                player.deck.Remove(cardDrawn);

                LinkAnimation(GM.animationManager.MoveCard(actionId, photonId, cardDrawn.instanceId, player.currentHolder.handGrid.value.position, player.currentHolder.handGrid.value.gameObject));

                AudioManager.singleton.Play(SoundEffectType.DRAW_CARD);

                if (GM.isMultiplayer && player.isLocal)
                {
                    MultiplayerManager.singleton.SendDraw(photonId, actionId, cardOrigin, effectOrigin);
                }
            }
            else if (player.deck.Count == 0 && player.discardCards.Count > 0)
            {
                if (player.isLocal)
                {
                    KAction shuffle = new A_Shuffle(photonId);

                    ////////////////////////////////////////////////////////////////////////////////////////////
                    foreach(Card c in player.playedCards)
                    {
                        foreach(CardEffect e in c.cardEffects)
                        {
                            if(e.type == EffectType.BEFORE_RECHARGE)
                            {
                                PushAction(new A_ExecuteEffect(c.instanceId, e.effectId, player.photonId));
                            }
                        }
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////

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
