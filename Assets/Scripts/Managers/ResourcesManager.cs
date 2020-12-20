using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Managers/Resource Manager")]
public class ResourcesManager : ScriptableObject
{
    public Card[] allCards;
    public DeckHolder[] allDecks;

    [System.NonSerialized]
    Dictionary<string, Card> cardDictionary = new Dictionary<string, Card>();

    public MainDataHolder dataHolder;

    int cardInstIndex;
    int actionIndex;
    int animationIndex;

    public void Init()
    {
        actionIndex = 0;
        cardInstIndex = 0;
        animationIndex = 0;

        cardDictionary.Clear();

        for (int i = 0; i < allCards.Length; i++)
        {
            cardDictionary.Add(allCards[i].name, allCards[i]);
        }
    }

    public void ResetMatch()
    {
        actionIndex = 0;
        cardInstIndex = 0;
        animationIndex = 0;
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

    public Card GetCardInfo(string id)
    {
        for (int i = 0; i < allCards.Length; i++)
        {
            if(allCards[i].name == id)
            {
                return allCards[i];
            }
        }

        return null;
    }

    private Card GetCard(string id)
    {
        Card result = null;

        cardDictionary.TryGetValue(id, out result);

        return result;
    }
}
