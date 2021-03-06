﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card")]
public class Card : ScriptableObject
{
    [System.NonSerialized]
    public int photonId;
    [System.NonSerialized]
    public int instanceId;
    [System.NonSerialized]
    public CardViz cardViz;
    [System.NonSerialized]
    public CardInstance cardPhysicalInst;

    [System.NonSerialized]
    public PlayerHolder owner;

    public int CardCost;

    public void MakeBorderActive()
    {
        if (!owner.isFloatingDefend(instanceId))
        {
            cardViz.cardBorder.color = Color.green;
        }

        cardViz.cardBackImage.gameObject.SetActive(false);
    }

    public void MakeBorderInactive()
    {
        cardViz.cardBorder.color = Color.black;
    }

    public void RevealCard()
    {
        cardViz.cardBackImage.gameObject.SetActive(false);
    }

    public int ModifiedCardCost()
    {
        int modifiedCost = CardCost;

        foreach (StatModification mod in statMods)
        {
            if(mod.stat_mod == StatType.ENERGY_COST)
            {
                modifiedCost = mod.modify(modifiedCost);
            }
        }

        if(owner != null)
        {
            foreach (StatModification mod in owner.statMods)
            {
                if (mod.stat_mod == StatType.ENERGY_COST)
                {
                    modifiedCost = mod.modify(modifiedCost);
                }
            }
        }

        return Math.Max(0, modifiedCost);
    }

    public string cardName;
    public Sprite art;
    public Sprite classIcon;

    public CardType cardType;

    public CardType GetCardType()
    {
        foreach (StatModification mod in statMods)
        {
            if (mod.stat_mod == StatType.QUICK_PLAY)
            {
                return GameManager.singleton.resourcesManager.dataHolder.quickPlayType;
            }
        }

        return cardType;
    }

    [TextArea(15, 20)]
    public string cardText;

    public List<CardTextMod> textMods = new List<CardTextMod>();

    public List<CardEffect> cardEffects = new List<CardEffect>();

    public List<Condition> conditions = new List<Condition>();

    [System.NonSerialized]
    public List<StatModification> statMods = new List<StatModification>();

    public void ResetCardEffects()
    {
        foreach (CardEffect effect in cardEffects)
        {
            effect.Reset();

            if (effect is CE_HandCheck)
            {
                ((CE_HandCheck)effect).linkedCardEffect.Reset();
            }
        }
    }

    public CardEffect GetEffect(int effectId)
    {
        for (int i = 0; i < cardEffects.Count; i++)
        {
            if (cardEffects[i].effectId == effectId)
            {
                return cardEffects[i];
            }

            if(cardEffects[i] is CE_HandCheck)
            {
                if(((CE_HandCheck)cardEffects[i]).linkedCardEffect.effectId == effectId)
                {
                    return ((CE_HandCheck)cardEffects[i]).linkedCardEffect;
                }
            }
        }

        return null;
    }

    public bool EffectsDone()
    {
        for (int i = 0; i < cardEffects.Count; i++)
        {
            if (cardEffects[i].type == EffectType.MAINTAIN || cardEffects[i].type == EffectType.ENDTURN || cardEffects[i].type == EffectType.STAT_MOD || cardEffects[i].type == EffectType.PREVAIL
                || cardEffects[i].type == EffectType.DISCARD_EFFECT || cardEffects[i].type == EffectType.HAND_EFFECT || cardEffects[i].type == EffectType.ENDTURNSTART || cardEffects[i].type == EffectType.STARTTURN)
                continue;

            if (!cardEffects[i].isDone)
                return false;
        }
        return true;
    }

    public CardTags[] cardTags;

    public bool HasTags(CardTags[] checks)
    {
        if (cardTags.Length < checks.Length)
            return false;

        return cardTags.Intersect(checks).Count() == checks.Count();
    }

    [System.NonSerialized]
    public bool isBroken = false;

    public void BreakeCard()
    {
        isBroken = true;
        cardViz.cardBrokenOverlay.gameObject.SetActive(true);
        cardViz.cardBorder.color = Color.red;

        foreach(CardEffect eff in cardEffects)
        {
            eff.OnLeavePlay();
        }

        iTween.PunchRotation(cardPhysicalInst.gameObject, new Vector3(0, 0, 0.5f), 0.5f);
        iTween.PunchScale(cardPhysicalInst.gameObject, new Vector3(0.3f, 0.3f), 0.5f);

        AudioManager.singleton.Play(SoundEffectType.CARD_BREAKE);
    }

    public void FixCard()
    {
        isBroken = false;
        cardViz.cardBrokenOverlay.gameObject.SetActive(false);
        cardViz.cardBorder.color = Color.black;
    }
}
