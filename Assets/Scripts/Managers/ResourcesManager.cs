﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Managers/Resource Manager")]
public class ResourcesManager : ScriptableObject
{
    public Card[] allCards;
    [System.NonSerialized]
    Dictionary<string, Card> cardDictionary = new Dictionary<string, Card>();

    public MainDataHolder dataHolder;
    public ScrollSelectionManager selectionManager;

    int cardInstIndex;
    int actionIndex;
    int animationIndex;

    public void Init()
    {
        actionIndex = 0;
        cardInstIndex = 0;
        cardDictionary.Clear();

        for (int i = 0; i < allCards.Length; i++)
        {
            cardDictionary.Add(allCards[i].name, allCards[i]);
        }
    }

    public int GetActionIndex()
    {
        actionIndex++;
        return  int.Parse(GameManager.singleton.localPlayer.photonId.ToString() + actionIndex.ToString());
    }
    
    public int GetAnimationIndex()
    {
        animationIndex++;
        return  int.Parse(GameManager.singleton.localPlayer.photonId.ToString() + animationIndex.ToString());
    }

    public Card GetCardInstance(string id)
    {
        Card originalCard = GetCard(id);

        if (originalCard == null)
            return null;

        Card newInst = Instantiate(originalCard);
        newInst.name = originalCard.name;
        newInst.instanceId = cardInstIndex;

        cardInstIndex++;

        return newInst;
    }

    private Card GetCard(string id)
    {
        Card result = null;

        cardDictionary.TryGetValue(id, out result);

        return result;
    }
}
