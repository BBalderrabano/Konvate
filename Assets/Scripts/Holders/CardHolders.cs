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

    public void LoadPlayer(PlayerHolder h)
    {
        foreach (Card c in h.playedCards)
        {
            c.cardViz.gameObject.transform.SetParent(playedGrid.value.transform);
        }
        
        foreach (Card c in h.handCards)
        {
            c.cardViz.gameObject.transform.SetParent(handGrid.value.transform);
        }
    }
}
