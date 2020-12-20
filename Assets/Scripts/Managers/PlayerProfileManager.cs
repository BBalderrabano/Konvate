using SO;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class PlayerProfileManager : MonoBehaviour
{
    public StringVariable playerName;

    public ResourcesManager RM;

    public TMP_InputField name_selection;
    public ScrollDeckSelector deck_selector;
    public TMP_Dropdown collection_selector;

    PlayerProfile profile;

    [System.NonSerialized]
    public int deck_saved_index = 0;

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
            PlayerPrefs.SetString("deck_name", RM.allDecks[0].deckName);
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

        for (int i = 0; i < RM.allDecks.Length; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(RM.allDecks[i].deckName);

            collection_selector.options.Add(option);

            if (RM.allDecks[i].deckName == profile.deckName)
            {
                deck_saved_index = i;
            }
        }

        collection_selector.value = deck_saved_index;
        collection_selector.captionText.text = RM.allDecks[deck_saved_index].deckName;

        profile.cardIds = RM.allDecks[deck_saved_index].cardIds;
        profile.playerName = name_selection.text;

        deck_selector.Init(RM.allDecks.ToList(), deck_saved_index);
    }

    public void SelectDeck()
    {
        SelectDeck(deck_saved_index);
    }

    public void SelectDeck(int index)
    {
        profile.cardIds = RM.allDecks[index].cardIds;
        profile.deckName = RM.allDecks[index].deckName;

        collection_selector.value = index;
        collection_selector.captionText.text = RM.allDecks[index].deckName;

        if (PlayerPrefs.HasKey("deck_name"))
        {
            PlayerPrefs.SetString("deck_name", RM.allDecks[index].deckName);
        }
        else
        {
            PlayerPrefs.SetString("deck_name", RM.allDecks[index].deckName);
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