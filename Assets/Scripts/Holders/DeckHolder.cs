using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Holders/Deck Data Holder")]
public class DeckHolder : ScriptableObject
{
    public string deckName;
    public string[] cardIds;

    public Sprite deckArt;
    public Sprite deckIcon;

    [TextArea(15, 20)]
    public string deckDesc;
}
