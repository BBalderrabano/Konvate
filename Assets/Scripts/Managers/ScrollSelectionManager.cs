﻿using DanielLochner.Assets.SimpleScrollSnap;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSelectionManager : MonoBehaviour
{
    public bool isActive = false;

    public static ScrollSelectionManager singleton;
    public SimpleScrollSnap sss;
    public GameObject cardPrefab;
    public GameObject selectorHolder;

    List<Card> listOfCards = new List<Card>();
    List<Card> cardsSelected = new List<Card>();

    public Button acceptButton;
    public Button addCardButton;
    public Button hideButton;
    public Button closeButton;

    A_CardSelection callback;
    string description;

    bool isMultipleSelection = false;
    bool isVisual = false;

    int maxSelected = 0;
    int minSelected = 1;

    int selectedAmount = 0;

    bool visible = true;

    public void Start()
    {
        if (singleton == null)
        {
            singleton = this;
        }

        gameObject.SetActive(false);
    }

    public void SelectCards(List<Card> cards, string description, bool isVisual, bool isMultiple = false, int minSelected = 0, int maxSelected = 0, A_CardSelection callback = null)
    {
        selectedAmount = 0;
        listOfCards.Clear();

        visible = true;
        this.isVisual = isVisual;

        isMultipleSelection = isMultiple;
        this.minSelected = minSelected;
        this.maxSelected = maxSelected;

        this.callback = callback;

        if (cards == null || cards.Count <= 0)
        {
            CloseSelection();
            return;
        }
        else
        {
            gameObject.SetActive(true);
            isActive = true;
        }

        if (isVisual)
        {
            addCardButton.gameObject.SetActive(false);
            acceptButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(true);
        }
        else
        {
            if (isMultiple)
            {
                addCardButton.gameObject.SetActive(true);
            }
            else
            {
                addCardButton.gameObject.SetActive(false);
            }

            acceptButton.gameObject.SetActive(true);
            closeButton.gameObject.SetActive(false);
        }

        hideButton.gameObject.SetActive(true);
        selectorHolder.gameObject.SetActive(true);

        if(description != null)
        {
            this.description = description;
            WarningPanel.singleton.ShowWarning(this.description, true);
        }

        while(sss.NumberOfPanels > 0)
        {
            sss.Remove(0);
        }
        
        foreach (Card c in cards)
        {
            GameObject newCard = Instantiate(cardPrefab) as GameObject;
            CardViz viz = newCard.GetComponent<CardViz>();

            viz.LoadCardViz(c);
            viz.card = c;

            sss.AddToBack(newCard, false);
            listOfCards.Add(c);
        }
    }

    internal void YesNoSelection(Card card, string description, A_CardSelection a_CardSelection)
    {
        throw new NotImplementedException();
    }

    public void OnAcceptSelection()
    {
        if(selectedAmount < minSelected)
        {
            WarningPanel.singleton.ShowWarning((minSelected - selectedAmount) > 1 ? "Debes elegir " + (minSelected - selectedAmount) + " cartas mas" : "Debes elegir 1 carta mas");
        }
        else
        {
            if (!isMultipleSelection)
            {
                OnCardSelected();
                CloseSelection();
            }
            else
            {
                CloseSelection();
            }
        }
    }

    public void OnCardSelected()
    {
        int index = sss.CurrentPanel;
        Card current = listOfCards[index];

        Transform border = sss.Panels[index].transform.GetChild(0).Find("Border");

        if (cardsSelected.Contains(current))
        {
            selectedAmount--;
            cardsSelected.Remove(current);

            if (border != null)
            {
                border.GetComponent<Image>().color = Color.black;
            }
        }
        else
        {
            if (maxSelected != 0 && selectedAmount >= maxSelected)
            {
                WarningPanel.singleton.ShowWarning("No puedes elegir mas cartas");
                return;
            }

            selectedAmount++;
            cardsSelected.Add(current);

            if (border != null)
            {
                border.GetComponent<Image>().color = Color.green;
            }
        }
    }

    public void ToggleSelection()
    {
        if (visible)
        {
            selectorHolder.SetActive(false);
            visible = false;
        }
        else
        {
            if (description != null)
            {
                WarningPanel.singleton.ShowWarning(this.description, false);
            }

            selectorHolder.SetActive(true);
            visible = true;
        }
    }

    public void CloseSelection()
    {
        if (!isVisual)
        {
            int[] cardIds = new int[cardsSelected.Count];

            for (int i = 0; i < cardsSelected.Count; i++)
            {
                cardIds[i] = cardsSelected[i].instanceId;
            }

            if (callback != null)
            {
                callback.DoneSelecting(cardIds);
            }
        }

        isActive = false;
        gameObject.SetActive(false);
    }
}
