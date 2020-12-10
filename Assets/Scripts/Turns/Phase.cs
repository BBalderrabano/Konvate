using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Turns/Phase")]
public abstract class Phase : ScriptableObject
{
    protected GameManager GM
    {
        get { return GameManager.singleton; }
    }

    [System.NonSerialized]
    public int phaseIndex;

    public string phaseDescription;

    public bool forcePlayerControlChange;

    #region Player Sync

    List<SyncSignal> playerSync = null;

    public void InitPlayerSync()
    {
        if (playerSync == null)
        {
            playerSync = new List<SyncSignal>();

            foreach (PlayerHolder p in GM.allPlayers)
            {
                playerSync.Add(new SyncSignal(p.photonId));
            }
        }
    }

    public bool PlayersAreReady()
    {
        for (int i = 0; i < playerSync.Count; i++)
        {
            if (!playerSync[i].isReady)
            {
                return false;
            }
        }

        return true;
    }

    public bool CheckPlayerIsReady(int photonId)
    {
        for (int i = 0; i < playerSync.Count; i++)
        {
            if (playerSync[i].photonId == photonId)
            {
                return playerSync[i].isReady;
            }
        }

        return false;
    }

    public void ResetPlayerSync()
    {
        for (int i = 0; i < playerSync.Count; i++)
        {
            playerSync[i].isReady = false;
        }
    }

    public void ChangePlayerSync(int photonId, bool value)
    {
        for (int i = 0; i < playerSync.Count; i++)
        {
            if(playerSync[i].photonId == photonId)
            {
                playerSync[i].isReady = value;
                return;
            }
        }
    }
    #endregion

    public abstract bool IsComplete();

    [System.NonSerialized]
    protected bool isInit = false;

    public abstract void OnTurnButtonHold();

    public abstract void OnTurnButtonPress();

    public virtual void OnStartPhase()
    {
        ResetPlayerSync();
    }

    public abstract void OnEndPhase();

    public abstract bool CanPlayCard(Card c);

    public abstract void OnPlayCard(Card c);
    
    public abstract void OnPhaseControllerChange(int photonId);

}
