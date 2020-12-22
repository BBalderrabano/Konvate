using System.Linq;
using UnityEngine;

public class A_Discard : KAction
{
    int cardTarget;
    bool waitForAnimation;

    public A_Discard(int cardId, int photonId, bool waitForAnimation = false, int actionId = -1, int cardOriginId = 0, int effectOriginId = 0) : base(photonId, actionId)
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

            if (player.handCards.Contains(card))
            {
                card.ResetCardEffects();
            }

            card.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.discardLogic);
            card.MakeBorderInactive();

            player.handCards.Remove(card);
            player.playedCards.Remove(card);
            player.deck.Remove(card);
            player.discardCards.Add(card);

            foreach (CardTextMod text_mod in card.textMods)
            {
                if (text_mod.isTemporary)
                {
                    text_mod.OnExpire();
                }
            }

            card.textMods.RemoveAll(a => a.isTemporary);

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
