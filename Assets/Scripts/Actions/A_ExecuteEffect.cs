using UnityEngine;

public class A_ExecuteEffect : KAction
{
    public Card owner;
    public CardEffect effect;

    float time = 0;

    bool makeActive = true;

    public A_ExecuteEffect(int cardId, int effectId, int photonId, int actionId = -1) : base(photonId, actionId)
    {
        owner = GM.GetPlayerHolder(photonId).GetCard(cardId);

        effect = owner.GetEffect(effectId);

        effect.parentAction = this;

        cardOrigin = cardId;
        effectOrigin = effectId;

        makeActive = true;
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

        return isInit && LinkedActionsReady() && AnimationsAreReady() && effectPreviewTimeout && !effect.isAnimatingCombo;
    }

    public override void OnComplete()
    {
        readyToRemove = true;

        effect.Finish();

        if (makeActive && owner.EffectsDone() && effect.type != EffectType.STARTTURN && !owner.isBroken)
        {
            owner.MakeBorderActive();
        }
    }

    public A_ExecuteEffect MakeActiveOnComplete(bool makeActive)
    {
        this.makeActive = makeActive;
        return this;
    }
}
