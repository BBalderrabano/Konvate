using SO;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSelectionManager : MonoBehaviour
{
    public StringVariable playerName;
    public List<DeckHolder> available_decks = new List<DeckHolder>();

    public TMP_InputField name_selection;
    public ScrollDeckSelector deck_selector;
    public TMP_Dropdown collection_selector;

    PlayerProfile profile;
    int deck_saved_index = 0;

    private void Awake()
    {
        deck_saved_index = 0;

        profile = Resources.Load("PlayerProfile") as PlayerProfile;

        if (PlayerPrefs.HasKey("deck_name"))
        {
            profile.deckName = PlayerPrefs.GetString("deck_name");
        }
        else
        {
            PlayerPrefs.SetString("deck_name", available_decks[0].deckName);
        }

        if (PlayerPrefs.HasKey("player_name"))
        {
           name_selection.text = PlayerPrefs.GetString("player_name");
        }
        else
        {
            PlayerPrefs.SetString("player_name", name_selection.text);
        }

        collection_selector.ClearOptions();


        for (int i = 0; i < available_decks.Count; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(available_decks[i].deckName);

            collection_selector.options.Add(option);

            if (available_decks[i].deckName == profile.deckName)
            {
                deck_saved_index = i;
            }
        }

        collection_selector.value = deck_saved_index;
        collection_selector.captionText.text = available_decks[deck_saved_index].deckName;

        profile.cardIds = available_decks[deck_saved_index].cardIds;
        profile.playerName = name_selection.text;

        deck_selector.Init(available_decks, deck_saved_index);
    }

    public void SelectDeck(int index)
    {
        profile.cardIds = available_decks[index].cardIds;
        profile.deckName = available_decks[index].deckName;

        if (PlayerPrefs.HasKey("deck_name"))
        {
            PlayerPrefs.SetString("deck_name", available_decks[index].deckName);
        }
        else
        {
            PlayerPrefs.SetString("deck_name", available_decks[index].deckName);
        }
    }

    public void SelectName(string new_name)
    {
        profile.playerName = new_name;

        if (PlayerPrefs.HasKey("player_name"))
        {
            PlayerPrefs.SetString("player_name", new_name);
        }
        else
        {
            PlayerPrefs.SetString("player_name", new_name);
        }
    }
}