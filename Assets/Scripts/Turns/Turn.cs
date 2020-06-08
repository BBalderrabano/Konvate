using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[CreateAssetMenu(menuName = "Turns/Turn")]
public class Turn : ScriptableObject
{
    [System.NonSerialized]
    public int index = 0;
    public PhaseVariable currentPhase;
    public Phase[] phases;

    public Phase GetPhaseByIndex(int phaseIndex)
    {
        return phases[phaseIndex];
    }

    public PlayerHolder localPlayer;
    public PlayerHolder clientPlayer;

    public PlayerHolder offensivePlayer;
    public GameObject offensiveChip;

    public bool IsOffensivePlayer(int photonId)
    {
        if (offensivePlayer == null)
            return false;

        return offensivePlayer.photonId == photonId;
    }

    public int turnCount = 0;

    public bool Execute()
    {
        bool result = false;
        bool phaseDone = false;

        if(currentPhase.value != phases[index])
        {
            currentPhase.value = phases[index];
            phases[index].OnStartPhase();

            if (GameManager.singleton.isMultiplayer)
            {
                MultiplayerManager.singleton.SetCurrentPhase(index);
            }

            return false;
        }
        else
        {
            phaseDone = phases[index].IsComplete();
        }

        if (phaseDone)
        {
            phases[index].OnEndPhase();

            index++;

            if(index > phases.Length - 1)
            {
                index = 0;
                result = true;
            }
        }

        return result;
    }
}
