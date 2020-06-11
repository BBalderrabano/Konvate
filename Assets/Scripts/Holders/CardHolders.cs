using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

[CreateAssetMenu(menuName = "Holders/Card Holder")]
public class CardHolders : ScriptableObject
{
    public TransformVariable deckGrid;
    public TransformVariable handGrid;
    public TransformVariable playedGrid;
    public TransformVariable discardGrid;
    public TransformVariable bleedChipHolder;
    public TransformVariable combatChipHolder;
    public TransformVariable poisonChipHolder;
    public TransformVariable playedCombatChipHolder;
    public TransformVariable playedPoisonChipHolder;
}
