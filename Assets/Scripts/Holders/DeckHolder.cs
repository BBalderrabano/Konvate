using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Holders/Deck Data Holder")]
public class DeckHolder : ScriptableObject
{
    public string deckName;
    public string[] cardIds;
}
