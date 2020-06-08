using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CatchCo;

public class CardViz : MonoBehaviour
{
    public TMPro.TextMeshProUGUI cardName;
    public TMPro.TextMeshProUGUI cardText;
    public Image art;
    public Text cardCost;
    public Image quickPlayIcon;
    public Image classIcon;
    public Image cardBackImage;
    public Image cardBorder;

    public Card card;

    #if UNITY_EDITOR
    [ExposeMethodInEditor]
    public void InpsectorLoadCard()
    {
        LoadCardViz(card);
    }
    #endif

    public void LoadCard(Card c) {
        if (c == null) {
            return;
        }

        c.cardViz = this;
        card = c;

        c.cardType.onSetType(this);

        cardName.text = c.cardName;
        cardText.text = c.cardText;
        cardCost.text = c.ModifiedCardCost().ToString();
        art.sprite = c.art;
        classIcon.sprite = c.classIcon;

        if (c.GetCardType() is QuickPlay)
        {
            quickPlayIcon.gameObject.SetActive(true);
        }
        else
        {
            quickPlayIcon.gameObject.SetActive(false);
        }

        ModifyCostColor(c);
    }
    
    public void LoadCardViz(Card c)
    {
        cardName.text = c.cardName;
        cardText.text = c.cardText;
        cardCost.text = c.ModifiedCardCost().ToString();
        art.sprite = c.art;
        classIcon.sprite = c.classIcon;

        if (c.GetCardType() is QuickPlay)
        {
            quickPlayIcon.gameObject.SetActive(true);
        }
        else
        {
            quickPlayIcon.gameObject.SetActive(false);
        }

        ModifyCostColor(c);
    }

    public void ModifyCostColor(Card c)
    {
        int modifiedCost = c.ModifiedCardCost();

        if (c.CardCost > modifiedCost)
        {
            cardCost.color = Color.green;
        }
        else if (c.CardCost < modifiedCost)
        {
            cardCost.color = Color.red;
        }
        else if (c.CardCost == modifiedCost)
        {
            cardCost.color = Color.black;
        }
    }

    public void RefreshStats()
    {
        LoadCardViz(card);
        ModifyCostColor(card);
    }
}
