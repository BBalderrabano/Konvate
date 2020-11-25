﻿using UnityEngine;
using System.Diagnostics;

public class A_ExecuteEffect : Action
{
    Card owner;
    CardEffect effect;

    float time = 0;

    public A_ExecuteEffect(int cardId, int effectId, int photonId, int actionId = -1) : base(GameManager.singleton.localPlayer.photonId, actionId)
    {
        owner = GM.GetPlayerHolder(photonId).GetCard(cardId);
        effect = owner.GetEffect(effectId);
        effect.parentAction = this;

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
            time = 0;

            GM.PreviewCard(effect.card);
            effect.Execute();

            isInit = true;
        }

        ExecuteLinkedAction(t);

        time += t;
    }

    public override bool IsComplete()
    {
        if (owner.isBroken)
            return true;

        bool effectPreviewTimeout = time > Settings.CARD_EFFECT_MIN_PREVIEW;

        if (effect.skipsEffectPreview)
        {
            effectPreviewTimeout = true;
        }

        return isInit && LinkedActionsReady() && AnimationsAreReady() && effectPreviewTimeout;
    }

    public override void OnComplete()
    {
        readyToRemove = true;

        effect.Finish();

        if (owner.EffectsDone() && effect.type != EffectType.STARTTURN && !owner.isBroken)
        {
            owner.MakeBorderActive();
        }
    }
}
