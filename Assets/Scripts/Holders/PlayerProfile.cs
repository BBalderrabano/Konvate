using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Profile")]
public class PlayerProfile : ScriptableObject
{
    public string deckName;
    public string[] cardIds;
    public string playerName;
}
