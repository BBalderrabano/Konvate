using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionNavigator : MonoBehaviour
{
    public GameObject cardPrefab;

    public ScrollRect cardCollectionScroll;

    public List<DeckPreviewHolder> deckPreviews = new List<DeckPreviewHolder>();

    public PlayerProfileManager playerProfileManager;

    public ResourcesManager resourcesManager;

    List<CollectionCardViz> cardsViz = new List<CollectionCardViz>();

    void Start()
    {
        cardsViz.Clear();

        cardsViz.Add(cardPrefab.GetComponent<CollectionCardViz>());

        for (int i = 0; i < 19; i++)
        {
            GameObject go = Instantiate(cardPrefab, cardPrefab.transform.parent);
            cardsViz.Add(go.GetComponent<CollectionCardViz>());
        }

        LoadDeck(playerProfileManager.deck_saved_index);
    }

    public void LoadCollectionMenu()
    {
        LoadDeck(playerProfileManager.deck_saved_index);
        cardCollectionScroll.verticalNormalizedPosition = 1;
    }

    public void LoadDeck(int deck_index)
    {
        DeckHolder loadedDeck = playerProfileManager.available_decks[deck_index];

        foreach (DeckPreviewHolder dp in deckPreviews)
        {
            dp.Populate(loadedDeck.deckName, loadedDeck.deckArt, loadedDeck.deckIcon);
        }

        for (int i = 0; i < cardsViz.Count; i++)
        {
            Card c = resourcesManager.GetCardInfo(loadedDeck.cardIds[i]);

            if (c == null)
            {
                Debug.LogError("Error mientras se cargaba una carta en el CollectionNavigator");
                break;
            }

            cardsViz[i].viz.LoadCardViz(c);
        }
    }
}
