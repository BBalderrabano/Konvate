using System.Linq;

public class A_DrawSpecific : KAction
{
    Card cardDrawn = null;
    int targetCardId;

    public A_DrawSpecific(int photonId, int targetCard, int actionId = -1, int cardId = 0, int effectId = 0) : base(photonId, actionId)
    {
        targetCardId = targetCard;
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

            cardDrawn = GM.GetCard(targetCardId);

            foreach (CardTextMod text_mod in cardDrawn.textMods)
            {
                if (text_mod.isTemporary)
                {
                    text_mod.OnExpire();
                }
            }

            cardDrawn.textMods.RemoveAll(a => a.isTemporary);

            cardDrawn.ResetCardEffects();

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
            player.discardCards.Remove(cardDrawn);

            LinkAnimation(GM.animationManager.MoveCard(actionId, photonId, cardDrawn.instanceId, player.currentHolder.handGrid.value.position, player.currentHolder.handGrid.value.gameObject));

            AudioManager.singleton.Play(SoundEffectType.DRAW_CARD);
          
            isInit = true;
        }
    }

    public override bool IsComplete()
    {
        return isInit && AnimationsAreReady() && LinkedActionsReady();
    }
}
