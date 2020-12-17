using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CollectionNavigator : MonoBehaviour
{
    public GameObject cardPrefab;

    public GameObject cardPreview;
    CardViz cardPreview_viz;
    Vector3 cardPreviewPos;
    Vector3 cardPreviewScale;

    public ScrollRect cardCollectionScroll;

    public Toggle showCardCopies;

    public List<DeckPreviewHolder> deckPreviews = new List<DeckPreviewHolder>();

    public PlayerProfileManager playerProfileManager;

    public ResourcesManager resourcesManager;

    List<CollectionCardViz> cardsViz = new List<CollectionCardViz>();

    CanvasGroup canvasGroup;

    void Start()
    {
        cardsViz.Clear();

        cardsViz.Add(cardPrefab.GetComponent<CollectionCardViz>());

        for (int i = 0; i < 19; i++)
        {
            GameObject go = Instantiate(cardPrefab, cardPrefab.transform.parent);
            cardsViz.Add(go.GetComponent<CollectionCardViz>());
        }

        canvasGroup = GetComponent<CanvasGroup>();

        cardPreview_viz = cardPreview.GetComponent<CardViz>();

        cardPreviewPos = cardPreview.transform.position;
        cardPreviewScale = cardPreview.transform.localScale;

        cardPreview.gameObject.SetActive(false);
        
        LoadDeck(playerProfileManager.deck_saved_index);
    }

    public void PreviewCard(Card c, Vector2 position)
    {
        LeanTween.cancel(cardPreview);

        cardPreview.SetActive(true);
        cardPreview.transform.position = position;
        cardPreview.transform.localScale = cardPrefab.transform.localScale;

        LeanTween.move(cardPreview, cardPreviewPos, 0.3f);
        LeanTween.scale(cardPreview, cardPreviewScale, 0.3f);

        cardPreview_viz.LoadCardViz(c);
    }

    public void LoadDeck()
    {
        cardPreview.gameObject.SetActive(false);

        LoadDeck(playerProfileManager.deck_saved_index);
    }

    public void LoadDeck(int deck_index)
    {
        cardCollectionScroll.verticalNormalizedPosition = 1;

        DeckHolder loadedDeck = playerProfileManager.available_decks[deck_index];

        foreach (DeckPreviewHolder dp in deckPreviews)
        {
            dp.Populate(loadedDeck.deckName, loadedDeck.deckArt, loadedDeck.deckIcon);
        }

        Array.Sort(loadedDeck.cardIds, StringComparer.InvariantCulture);

        for (int i = 0; i < cardsViz.Count; i++)
        {
            if(i < loadedDeck.cardIds.Length)
            {
                Card c = resourcesManager.GetCardInfo(loadedDeck.cardIds[i]);

                if (c == null)
                {
                    Debug.LogError("Error mientras se cargaba una carta en el CollectionNavigator");
                    break;
                }

                cardsViz[i].viz.LoadCard(c);
                cardsViz[i].gameObject.SetActive(true);
            }
            else
            {
                cardsViz[i].gameObject.SetActive(false);
            }
        }

        ToggleCardCopies(showCardCopies, true);
    }

    public void ToggleCardCopies(bool toggle)
    {
        ToggleCardCopies(toggle, false);
    }

    public void ToggleCardCopies(bool toggle, bool deckChanged)
    {
        string lastCardName = "";

        for (int i = 0; i < cardsViz.Count; i++)
        {
            cardsViz[i].amount_counter.gameObject.SetActive(!toggle);

            if (lastCardName != cardsViz[i].viz.cardName.text)
            {
                if (deckChanged)
                {
                    cardsViz[i].amount_counter.text = "x" + cardsViz.FindAll(a => a.viz.cardName.text == cardsViz[i].viz.cardName.text).Count;
                }

                lastCardName = cardsViz[i].viz.cardName.text;
            }
            else
            {
                cardsViz[i].gameObject.SetActive(toggle);
            }
        }
    }
}
