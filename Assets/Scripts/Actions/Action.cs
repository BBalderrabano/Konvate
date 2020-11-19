using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Action
{
    #region Variables
    protected GameManager GM
    {
        get { return GameManager.singleton; }
    }

    public bool isInit = false;
    public bool readyToRemove = false;

    public int photonId;
    public int actionId;

    protected int effectOrigin;
    protected int cardOrigin;

    List<SyncSignal> playerSync;
    #endregion

    public Action(int photonId, int actionId = -1)
    {
        this.isInit = false;
        this.readyToRemove = false;

        if(photonId < 0)
        {
            this.photonId = GM.localPlayer.photonId;
        }
        else
        {
            this.photonId = photonId;
        }

        if (actionId < 0)
        {
            this.actionId = GM.resourcesManager.GetActionIndex();
        }
        else
        {
            this.actionId = actionId;
        }

        this.playerSync = new List<SyncSignal>();

        foreach (PlayerHolder p in GM.allPlayers)
        {
            playerSync.Add(new SyncSignal(p.photonId));
        }
    }

    #region Linked Actions
    public List<Action> linkedActions = new List<Action>();

    public virtual void PushActions(List<Action> actionsToPush)
    {
        linkedActions.AddRange(actionsToPush);
    }

    public virtual void PushAction(Action actionToPush)
    {
        linkedActions.Add(actionToPush);
    }

    public virtual Action GetLinkedAction(int actionId)
    {
        for (int i = 0; i < linkedActions.Count; i++)
        {
            if (linkedActions[i].actionId == actionId)
            {
                return linkedActions[i];
            }

            if (linkedActions[i].GetLinkedAction(actionId) != null)
                return linkedActions[i].GetLinkedAction(actionId);
        }

        return null;
    }

    public virtual void ExecuteLinkedAction(float t)
    {
        for (int i = 0; i < linkedActions.Count; i++)
        {
            Action current = linkedActions[i];
        
            if (!current.IsComplete())
            {
                current.Execute(t);

                if (!current.Continue())
                {
                    break;
                }
            }
            else
            {
                if (!current.readyToRemove)
                {
                    current.OnComplete();
                }
            }
        }

        linkedActions.RemoveAll(a => a.readyToRemove);
    }

    #endregion

    public List<Animation> linkedAnimations = new List<Animation>();

    public virtual bool PlayersAreReady()
    {
        for (int i = 0; i < playerSync.Count; i++)
        {
            if (!playerSync[i].isReady)
                return false;
        }

        return true;
    }

    public virtual void PlayerIsReady(int photonId)
    {
        for (int i = 0; i < playerSync.Count; i++)
        {
            if (playerSync[i].photonId == photonId)
            {
                playerSync[i].isReady = true;
                break;
            }
        }
    }

    public virtual bool LinkedActionsReady()
    {
        for (int i = 0; i < linkedActions.Count; i++)
        {
            if (linkedActions[i] == this)
                continue;

            if (!linkedActions[i].IsComplete())
                return false;
        }

        return true;
    }

    public virtual bool AnimationsAreReady()
    {
        for (int i = 0; i < linkedAnimations.Count; i++)
        {
            if (!linkedAnimations[i].isDone())
                return false;
        }

        return true;
    }

    public virtual void LinkAnimation(Animation anim)
    {
        if(anim != null)
        {
            linkedAnimations.Add(anim);
        }
    }

    public virtual Animation GetAnimation(int animId)
    {
        for (int i = 0; i < linkedAnimations.Count; i++)
        {
            if (linkedAnimations[i].animId == animId)
                return linkedAnimations[i];
        }

        return null;
    }

    public virtual void CompleteAnimation(int animId, int photonId)
    {
        bool animationsComplete = true;

        for (int i = 0; i < linkedAnimations.Count; i++)
        {
            if (linkedAnimations[i].animId == animId)
            {
                linkedAnimations[i].OnComplete();
            }
            if (!linkedAnimations[i].isDone())
            {
                animationsComplete = false;
            }
        }

        if (animationsComplete && photonId > -1)
        {
            PlayerIsReady(photonId);
        }
    }

    public abstract bool Continue();

    public abstract bool IsComplete();

    public abstract void Execute(float t);

    public virtual void OnComplete()
    {
        if (effectOrigin != 0 && cardOrigin != 0)
        {
            PlayerHolder player = GM.GetPlayerHolder(photonId);
            Card effectCard = player.GetCard(cardOrigin);
            CardEffect effect = effectCard.GetEffect(effectOrigin);

            if (effect != null)
            {
                effect.Finish();
            }
        }

        readyToRemove = true;
    }
}

public class SyncSignal
{
    public int photonId;
    public bool isReady = false;

    public SyncSignal(int photonId)
    {
        this.photonId = photonId;
        this.isReady = false;
    }
}