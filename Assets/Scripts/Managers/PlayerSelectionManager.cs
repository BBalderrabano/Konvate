using SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionManager : MonoBehaviour
{
    public StringVariable playerName;
    public List<DeckHolder> available_decks = new List<DeckHolder>();

    public TMPro.TMP_InputField name_selection;
    public TMPro.TMP_Dropdown deck_selection;

    PlayerProfile profile;
    int deck_saved_index = 0;

    private void Start()
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

        deck_selection.ClearOptions();

        for (int i = 0; i < available_decks.Count; i++)
        {
            TMPro.TMP_Dropdown.OptionData option = new TMPro.TMP_Dropdown.OptionData(available_decks[i].deckName);

            deck_selection.options.Add(option);

            if (available_decks[i].deckName == profile.deckName)
            {
                deck_saved_index = i;
            }
        }

        deck_selection.value = deck_saved_index;
        deck_selection.captionText.text = available_decks[deck_saved_index].deckName;

        profile.cardIds = available_decks[deck_saved_index].cardIds;
        profile.playerName = name_selection.text;
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