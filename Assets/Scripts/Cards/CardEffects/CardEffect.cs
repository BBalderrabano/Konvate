using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public bool skipsEffectPreviewTime = false;

    [System.NonSerialized]
    public bool skipsCardPreview = false;

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public CardEffect Clone(Card effectOwner, bool isTemporary = true)
    {
        CardEffect clone = (CardEffect) this.Clone();

        clone.effectId = int.Parse(effectId.ToString() + GM.resourcesManager.GetUniqueId());
        clone.card = effectOwner;
        clone.isTemporary = isTemporary;

        return clone;
    }

    public virtual void Execute()
    {
        card.cardViz.cardBorder.color = Color.blue;
    }

    public virtual void OnLeavePlay() { }

    [System.NonSerialized]
    public bool isAnimatingLeanTween = false;

    [System.NonSerialized]
    public bool isAnimatingCombo = false;

    public bool IsCombo()
    {
        PlayerHolder player = card.owner;

        List<Card> cardCopies = player.playedCards.FindAll(a => a.cardName == card.cardName);

        if (cardCopies.Count > 1 && !GM.turn.comboTracker.Exists(a => a.card_name == card.cardName && a.photonId == card.owner.photonId))
        {
            float shakeAmt = 20f;
            float shakePeriodTime = 0.22f;
            float dropOffTime = 1f;

            isAnimatingCombo = true;

            foreach (Card c in cardCopies)
            {
                LTDescr shakeTween = LeanTween.rotateAroundLocal(c.cardPhysicalInst.gameObject, Vector3.up, shakeAmt, shakePeriodTime)
                    .setEase(LeanTweenType.easeShake)
                    .setLoopClamp()
                    .setRepeat(-1);

                LeanTween.value(c.cardPhysicalInst.gameObject, shakeAmt, 0f, dropOffTime).setOnUpdate(
                    (float val) => {
                        shakeTween.setTo(Vector3.right * val);

                        if(val <= 0)
                        {
                            this.isAnimatingCombo = false;
                        }
                    }
                ).setEase(LeanTweenType.easeOutQuad);
            }

            GM.turn.comboTracker.Add(new ComboTracker(card.owner.photonId, card.cardName));

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
    QUICK_COUNTER,
    DISCARD_EFFECT
}