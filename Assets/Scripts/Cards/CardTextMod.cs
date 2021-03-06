﻿using UnityEngine;

public abstract class CardTextMod : ScriptableObject
{
    [System.NonSerialized]
    public Card card;
    [System.NonSerialized]
    public string original_text;

    public string description;
    public string text_target;

    public bool isTemporary = false;

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public virtual void Init(Card c)
    {
        card = c;
        original_text = c.cardViz.cardText.text;
    }

    public abstract void UpdateText();

    public virtual void OnExpire()
    {
        card.cardViz.cardText.text = original_text;
    }
}
