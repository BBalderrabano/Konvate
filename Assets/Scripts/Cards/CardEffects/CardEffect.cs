using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffect : ScriptableObject, ICloneable
{
    protected GameManager GM
    {
        get { return GameManager.singleton; }
    }

    public bool isTemporary = false;

    public int priority = 1;

    [System.NonSerialized]
    public int effectId = 0;

    [System.NonSerialized]
    public bool isDone = false;

    public virtual void Reset()
    {
        this.isDone = false;
        this.parentAction = null;
    }

    public virtual void Finish()
    {
        this.isDone = true;
        this.parentAction = null;
    }

    [System.NonSerialized]
    public Card card;

    public EffectType type = EffectType.NONE;

    public CardTags[] effectTags;

    [System.NonSerialized]
    public A_ExecuteEffect parentAction;

    [System.NonSerialized]
    public bool skipsEffectPreview = false;

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public virtual void Execute()
    {
        card.cardViz.cardBorder.color = Color.blue;
    }

    public virtual void OnLeavePlay() { }
    
    public bool IsCombo()
    {
        PlayerHolder player = card.owner;

        List<Card> cardCopies = player.playedCards.FindAll(a => a.cardName == card.cardName);

        if (cardCopies.Count > 1)
        {
            float shakeAmt = 0.2f; // the degrees to shake
            float shakePeriodTime = 0.22f; // The period of each shake
            float dropOffTime = 1f; // How long it takes the shaking to settle down to nothing

            LTDescr shakeTween = LeanTween.rotateAroundLocal(card.cardPhysicalInst.gameObject, Vector3.right, shakeAmt, shakePeriodTime)
                .setEase(LeanTweenType.easeShake) // this is a special ease that is good for shaking
                .setLoopClamp()
                .setRepeat(-1);

            LeanTween.value(card.cardPhysicalInst.gameObject, shakeAmt, 0f, dropOffTime).setOnUpdate(
                (float val) => {
                    shakeTween.setTo(Vector3.right * val);
                }
            ).setEase(LeanTweenType.easeOutQuad);

            return true;
        }

        return false;
    }
}

public enum EffectType
{
    NONE,
    SPECIAL,
    PLACE,
    REMOVE,
    RESTORE,
    ENDTURNSTART,
    ENDTURN,
    MAINTAIN,
    ON_PLAY_START,
    STAT_MOD,
    PREVAIL,
    STARTTURN,
    ON_PLAY_END,
    HAND_EFFECT,
    BEFORE_RECHARGE,
    QUICK_COUNTER
}