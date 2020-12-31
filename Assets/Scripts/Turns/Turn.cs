using SO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Turns/Turn")]
public class Turn : ScriptableObject
{
    [System.NonSerialized]
    public int phaseIndex = 0;
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

    [System.NonSerialized]
    public List<CardEffect> startTurnEffects = new List<CardEffect>();
    [System.NonSerialized]
    public List<CardEffect> endTurnEffects = new List<CardEffect>();
    [System.NonSerialized]
    public List<ComboTracker> comboTracker = new List<ComboTracker>();

    public bool localInflictedBleed = false;
    public bool opponentInflictedBleed = false;

    public TransformVariable playerOneTurnLine;
    public TransformVariable playerTwoTurnLine;

    public bool playerInflictedBleed(int photonId)
    {
        if (GameManager.singleton.GetPlayerHolder(photonId).isLocal)
        {
            return localInflictedBleed;
        }
        else
        {
            return opponentInflictedBleed;
        }
    }


    public bool IsOffensivePlayer(int photonId)
    {
        if (offensivePlayer == null)
            return false;

        return offensivePlayer.photonId == photonId;
    }

    public int turnCount = 0;

    public void ResetMatch()
    {
        currentPhase.value = phases.Last();
        turnCount = 0;
        phaseIndex = 0;
        startTurnEffects.Clear();
        endTurnEffects.Clear();
        localInflictedBleed = false;
        opponentInflictedBleed = false;
    }

    public bool Execute()
    {
        bool result = false;
        bool phaseDone;

        if (currentPhase.value != phases[phaseIndex])
        {
            currentPhase.value = phases[phaseIndex];

            phases[phaseIndex].OnStartPhase();

            return false;
        }
        else
        {
            phaseDone = phases[phaseIndex].IsComplete();
        }

        if (phaseDone)
        {
            phases[phaseIndex].OnEndPhase();

            phaseIndex++;

            if(phaseIndex > phases.Length - 1)
            {
                phaseIndex = 0;
                result = true;
            }
        }

        PaintMiddleLine();

        return result;
    }

    void PaintMiddleLine()
    {
        GameManager GM = GameManager.singleton;

        PlayerHolder player = GM.localPlayer;
        PlayerHolder opponent = GM.GetOpponentHolder(player.photonId);

        if (GM.currentPlayer == null)
        {
            playerOneTurnLine.value.GetComponent<Image>().color = Color.white;
            playerTwoTurnLine.value.GetComponent<Image>().color = Color.white;
        }
        else 
        {
            if (GM.currentPlayer.isLocal)
            {
                playerOneTurnLine.value.GetComponent<Image>().color = Color.green;

                playerTwoTurnLine.value.GetComponent<Image>().color = Color.white;
            }
            else if (!GM.currentPlayer.isLocal)
            {
                playerTwoTurnLine.value.GetComponent<Image>().color = Color.green;

                playerOneTurnLine.value.GetComponent<Image>().color = Color.white;
            }

            if (currentPhase.value.CheckPlayerIsReady(player.photonId))
            {
                playerOneTurnLine.value.GetComponent<Image>().color = Color.red;
            }

            if (currentPhase.value.CheckPlayerIsReady(opponent.photonId))
            {
                playerTwoTurnLine.value.GetComponent<Image>().color = Color.red;
            }
        } 
    }
}
