using UnityEngine;
using System.Collections;

public class Anim_FuerzaEnNumeros : KAction
{
    float time = 0;
    readonly PlayerHolder player;
    readonly Card card;
    readonly int chip_amount;
    bool cardPlaced;

    public Anim_FuerzaEnNumeros(int photonId, int cardId, int chip_amount, int actionId = -1) : base(photonId, actionId)
    {
        player = GM.GetPlayerHolder(photonId);
        card = player.GetCard(cardId);
        this.chip_amount = chip_amount;
        cardPlaced = false;
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
            GM.PreviewCard(card, false);
        }

        time += t;
    }

    public override bool IsComplete()
    {
        if (isInit && AnimationsAreReady() && !cardPlaced && (time > Settings.CARD_EFFECT_MIN_PREVIEW))
        {
            LinkAnimation(GM.animationManager.PlaceChip(actionId, card.owner.photonId, card.instanceId, ChipType.COMBAT, chip_amount));
            cardPlaced = true;
            time = 0;

            return false;
        }
        else if (isInit && AnimationsAreReady() && cardPlaced && (time > Settings.CARD_EFFECT_MIN_PREVIEW))
        {
            card.MakeBorderActive();
            GM.AnimateHidePrivewCard();
            return true;
        }
        else
        {
            return false;
        }
    }
}
