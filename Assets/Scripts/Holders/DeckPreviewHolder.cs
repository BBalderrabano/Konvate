using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeckPreviewHolder : MonoBehaviour
{
    public TMP_Text deckName;
    public Image deckArt;
    public Image deckIcon;
    public DeckPreviewPosition deckPreviewPosition;

    public void Populate(string deck_name, Sprite art, Sprite icon)
    {
        deckName.text = deck_name;
        deckArt.sprite = art;
        deckIcon.sprite = icon;
    }
}

public enum DeckPreviewPosition
{
    CENTER = default,
    LEFT,
    LEFT_HIDDEN,
    RIGHT,
    RIGHT_HIDDEN
}