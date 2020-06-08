using DanielLochner.Assets.SimpleScrollSnap;
using SO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSelectionManager : MonoBehaviour
{    
    public List<Card> test = new List<Card>();

    public static ScrollSelectionManager singleton;
    public SimpleScrollSnap sss;
    public GameObject cardPrefab;
    public GameObject selectorHolder;

    List<Card> listOfCards = new List<Card>();

    public Button acceptButton;
    public Button addCardButton;
    public Button hideButton;
    public Button closeButton;

    public CardListVariable cardsSelected;
    public BoolVariable doneSelecting;

    CardEffect callback;

    string description;

    bool isMultipleSelection = false;
    bool isVisual = false;

    int maxSelected = 0;
    int minSelected = 1;

    int selectedAmount = 0;

    bool visible = true;

    bool waitingForOpponent = false;

    public void Start()
    {
        if (singleton == null)
        {
            singleton = this;
        }

        if(test.Count > 0)
        {
            SelectCards(test, "Cartas de prueba", true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (waitingForOpponent)
        {
            if (doneSelecting.value)
            {
                CloseSelection();
            }
        }
    }

    public void WaitForOpponent(CardEffect effect)
    {
        callback = effect;

        gameObject.SetActive(true);
        addCardButton.gameObject.SetActive(false);
        acceptButton.gameObject.SetActive(false);
        hideButton.gameObject.SetActive(false);
        selectorHolder.gameObject.SetActive(false);

        waitingForOpponent = true;
    }

    public void SelectCards(List<Card> cards, string description, bool isVisual, bool isMultiple = false, int minSelected = 0, int maxSelected = 0, CardEffect effect = null)
    {
        waitingForOpponent = false;

        selectedAmount = 0;
        listOfCards.Clear();
        cardsSelected.values.Clear();

        doneSelecting.value = false;
        visible = true;
        this.isVisual = isVisual;

        isMultipleSelection = isMultiple;
        this.minSelected = minSelected;
        this.maxSelected = maxSelected;

        callback = effect;

        if (cards == null || cards.Count <= 0)
        {
            CloseSelection();
            return;
        }
        else
        {
            gameObject.SetActive(true);
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

        if (cardsSelected.values.Contains(current))
        {
            selectedAmount--;
            cardsSelected.values.Remove(current);

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
            cardsSelected.values.Add(current);

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
        if (!waitingForOpponent && !isVisual)
        {
            int[] cardIds = new int[cardsSelected.values.Count];

            for (int i = 0; i < cardsSelected.values.Count; i++)
            {
                cardIds[i] = cardsSelected.values[i].instanceId;
            }

            MultiplayerManager.singleton.PlayerFinishCardSelection(cardIds);
        }

        waitingForOpponent = false;
        doneSelecting.value = true;

        if (callback != null)
        {
            callback.Execute();
        }

        gameObject.SetActive(false);
    }
}
