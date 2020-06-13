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

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public virtual void Execute()
    {
        card.cardViz.cardBorder.color = Color.blue;
    }

    public bool IsCombo()
    {
        PlayerHolder player = card.owner;

        List<Card> cardCopies = player.playedCards.FindAll(a => a.cardName == card.cardName);

        if (cardCopies.Count > 1 && cardCopies.Count < 3)
        {
            foreach (Card c in cardCopies)
            {
                iTween.ShakePosition(c.cardPhysicalInst.gameObject, iTween.Hash("x", 0.01f, "y", 0.01f, "time", 1.0f));
            }
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
    PLAY_START,
    STAT_MOD,
    PREVAIL,
    STARTTURN,
    PLAY_END
}