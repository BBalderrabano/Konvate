using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{

    public PlayerHolder player;
    public TMPro.TextMeshProUGUI bleedCount;
    public TMPro.TextMeshProUGUI energyCount;

    void Start()
    {
        player.playerUI = this;
        UpdateBloodChips();
    }

    public void UpdateEnergyCount()
    {
        if(player.photonId != GameManager.singleton.localPlayer.photonId && GameManager.singleton.turn.currentPhase.value is SetCardsPhase)
        {
            energyCount.text = "?";
        }
        else
        {
            energyCount.text = player.currentEnergy.value.ToString();
        }
    }

    public void UpdateBloodChips()
    {
        bleedCount.text = player.bleedCount.ToString();
    }

    public void UpdateAll()
    {
        UpdateBloodChips();
        UpdateEnergyCount();
    }
}
