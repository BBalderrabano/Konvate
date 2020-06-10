    using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Holders/Player Holder")]
public class PlayerHolder : ScriptableObject
{
    public string playerName;

    [System.NonSerialized]
    public int photonId = -1;

    [System.NonSerialized]
    public NetworkPrint print;

    public List<string> starting_deck = new List<string>();
    public List<Card> all_cards = new List<Card>();

    public Card GetCard(int instanceId)
    {
        for (int i = 0; i < all_cards.Count; i++)
        {
            if (all_cards[i].instanceId == instanceId)
                return all_cards[i];
        }

        return null;
    }

    public List<Card> deck = new List<Card>();
    public List<Card> handCards = new List<Card>();
    public List<Card> playedCards = new List<Card>();
    public List<Card> discardCards = new List<Card>();

    public int maxHealth;
    [System.NonSerialized]
    public int bleedCount;
    public bool IsDead()
    {
        return bleedCount <= 0;
    }

    int StartDrawAmount;

    public int ModifiedStartDrawAmount()
    {
        int modifiedCost = StartDrawAmount;

        foreach (Card c in all_cards)
        {
            foreach (StatModification mod in c.cardEffects.OfType<StatModification>())
            {
                if (mod.stat_mod == StatType.START_DRAW_AMOUNT)
                {
                    modifiedCost = mod.modify(modifiedCost);
                }
            }
        }


        return modifiedCost;
    }

    public PlayerUIManager playerUI;

    public bool isHumanPlayer;
    public bool isLocal;

    [System.NonSerialized]
    public CardHolders currentHolder;

    [System.NonSerialized]
    List<FloatingDefenseHolder> floatingDefends = new List<FloatingDefenseHolder>();

    public void addFloatingDefend(FloatingDefenseHolder holder)
    {
        floatingDefends.Add(holder);
        floatingDefends = floatingDefends.OrderBy(a => a.effect.priority).ToList();
    }

    public void removeFloatingDefend(FloatingDefenseHolder holder)
    {
        floatingDefends.Remove(holder);
    }

    public FloatingDefenseHolder getFloatingDefend(ChipType type)
    {
        List<FloatingDefenseHolder> check = new List<FloatingDefenseHolder>();

        foreach (FloatingDefenseHolder holder in floatingDefends)
        {
            if (holder.type == type || (holder.type == ChipType.COMBAT && type == ChipType.COMBAT_OFFENSIVE) || (holder.type == ChipType.OFFENSIVE && type == ChipType.COMBAT_OFFENSIVE))
                check.Add(holder);
        }

        if(check.Count > 0)
        {
            check.OrderBy(a => a.effect.priority);

            return check[0];
        }
        else
        {
            return null;
        }
    }

    public bool isFloatingDefend(int cardInstance)
    {
        for (int i = 0; i < floatingDefends.Count; i++)
        {
            if (floatingDefends[i].effect.card.instanceId == cardInstance)
                return true;
        }

        return false;
    }

    public void clearFloatingDefend()
    {
        floatingDefends.Clear();
    }

    public enum CardLocation
    {
        Hand, Play, Discard, Deck
    }

    public bool hasCardOnLocation(int instanceId, CardLocation location)
    {
        switch (location)
        {
            case CardLocation.Hand:
                for (int i = 0; i < handCards.Count; i++)
                {
                    if (handCards[i].instanceId == instanceId)
                        return true;
                }
                break;
            case CardLocation.Discard:
                for (int i = 0; i < discardCards.Count; i++)
                {
                    if (discardCards[i].instanceId == instanceId)
                        return true;
                }
                break;
            case CardLocation.Play:
                for (int i = 0; i < playedCards.Count; i++)
                {
                    if (playedCards[i].instanceId == instanceId)
                        return true;
                }
                break;
            case CardLocation.Deck:
                for (int i = 0; i < deck.Count; i++)
                {
                    if (deck[i].instanceId == instanceId)
                        return true;
                }
                break;
        }

        return false;
    }

    public CardInstance getCardOnLocation(int instanceId, CardLocation location)
    {
        switch (location)
        {
            case CardLocation.Hand:
                for (int i = 0; i < handCards.Count; i++)
                {
                    if (handCards[i].instanceId == instanceId)
                        return handCards[i].cardPhysicalInst;
                }
                break;
            case CardLocation.Discard:
                for (int i = 0; i < discardCards.Count; i++)
                {
                    if (discardCards[i].instanceId == instanceId)
                        return discardCards[i].cardPhysicalInst;
                }
                break;
            case CardLocation.Play:
                for (int i = 0; i < playedCards.Count; i++)
                {
                    if (playedCards[i].instanceId == instanceId)
                        return playedCards[i].cardPhysicalInst;
                }
                break;
            case CardLocation.Deck:
                for (int i = 0; i < deck.Count; i++)
                {
                    if (deck[i].instanceId == instanceId)
                        return deck[i].cardPhysicalInst;
                }
                break;
        }

        return null;
    }

    public SO.IntVariable currentEnergy;
    public int baseEnergy = 3;

    public bool playedQuickCard = false;

    public void Init()
    {
        baseEnergy = 3;
        maxHealth = 10;
        bleedCount = maxHealth;
        StartDrawAmount = 5;

        all_cards.Clear();
        deck.Clear();
        handCards.Clear();
        playedCards.Clear();
        discardCards.Clear();

        playerUI.UpdateAll();

        CreateBleedChips();
    }

    void CreateBleedChips()
    {
        GameObject prefab = GameManager.singleton.resourcesManager.dataHolder.bleedChipPrefab;

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject newInstance = Instantiate(prefab);
            Settings.SetParent( newInstance.transform, currentHolder.bleedChipHolder.value);
            newInstance.GetComponent<Chip>().owner = this;
        }
    }

    public void ResetMana()
    {
        currentEnergy.value = baseEnergy;
        VisualizeEnergy();
    }

    public void ChangeMana(int amount)
    {
        currentEnergy.value += amount;
        VisualizeEnergy();
    }

    private void VisualizeEnergy()
    {
        playerUI.UpdateEnergyCount();
    }

    public bool CanUseCard(Card c)
    {
        bool result = false;

        if (c.ModifiedCardCost() <= currentEnergy.value)
        {
            return true;
        }
        else
        {
            WarningPanel.singleton.ShowWarning("No tienes suficiente energia");
        }

        return result;
    }

    public void ModifyBloodChip(int amount)
    {
        bleedCount += amount;

        if(bleedCount > maxHealth)
        {
            bleedCount = maxHealth;
        }

        if(playerUI != null)
        {
            playerUI.UpdateBloodChips();
        }
    }
}
