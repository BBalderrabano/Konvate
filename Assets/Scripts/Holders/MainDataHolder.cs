using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

[CreateAssetMenu(menuName = "Holders/Main Data Holder")]
public class MainDataHolder : ScriptableObject
{
    public GameObject offensiveChip;

    public GameObject bleedChipPrefab;

    public GameObject cardPrefab;

    public CardLogic cardQuickPlayLogic;
    public CardLogic cardFaceDownLogic;
    public CardLogic handLogic;
    public CardLogic opponentHandLogic;
    public CardLogic playedLogic;
    public CardLogic discardLogic;
    public CardLogic deckLogic;

    public CardType quickPlayType;
    public CardType setPlayType;

    public BoolVariable carouselIsDone;
    public CardListVariable carouselSelection;
}