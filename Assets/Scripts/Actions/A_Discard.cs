using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class A_Discard : KAction
{
    int cardTarget;
    bool waitForAnimation;
    bool checkForDiscardEffects;
    bool hasDiscardEffect = false;

    PlayerHolder player;
    Card card;

    public A_Discard(int cardId, int photonId, bool checkForDiscardEffects = false, bool waitForAnimation = false, int actionId = -1, int cardOriginId = 0, int effectOriginId = 0) : base(photonId, actionId)
    {
        cardOrigin = cardOriginId;
        effectOrigin = effectOriginId;
        cardTarget = cardId;
        hasDiscardEffect = false;

        this.waitForAnimation = waitForAnimation;
        this.checkForDiscardEffects = checkForDiscardEffects;
    }

    public override bool Continue()
    {
        return waitForAnimation;
    }

    public override void Execute(float t)
    {
        if (!isInit)
        {
            player = GM.GetPlayerHolder(photonId);
            card = player.GetCard(cardTarget);

            if (player.handCards.Contains(card))
            {
                card.ResetCardEffects();

                if (checkForDiscardEffects && card.cardEffects.Exists(a => a.type == EffectType.DISCARD_EFFECT))
                {
                    hasDiscardEffect = true;
                }
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
        return LinkedActionsReady() && isInit && AnimationsAreReady();
    }

    public override void OnComplete()
    {
        if (hasDiscardEffect)
        {
            List<KAction> discardEffects = new List<KAction>();

            foreach (CardEffect eff in card.cardEffects.FindAll(a => a.type == EffectType.DISCARD_EFFECT))
            {
                discardEffects.Add(new A_ExecuteEffect(card.instanceId, eff.effectId, card.owner.photonId, this.actionId));
            }

            PushActions(discardEffects);

            GM.turn.turnFlags.AddFlag(new TurnFlag(card.owner.photonId, FlagDesc.DISCARD_AMOUNT_BY_EFF, 1));

            hasDiscardEffect = false;
        }
        else if(LinkedActionsReady())
        {
            base.OnComplete();
        }
    }
}
