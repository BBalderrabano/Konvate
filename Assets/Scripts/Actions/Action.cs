using System.Collections.Generic;

public abstract class Action
{
    public bool isInit = false;
    public bool readyToRemove = false;

    public int photonId;
    public int actionId;
    List<SyncSignal> playerSync;

    public List<Action> linkedActions = new List<Action>();
    public List<Animation> linkedAnimations = new List<Animation>();

    public bool forceContinue = false;  //TODO: Unelegant solution, needs improvement

    protected int effectOrigin;
    protected int cardOrigin;

    protected GameManager GM
    {
        get { return GameManager.singleton; }
    }

    public Action(int photonId, int actionId = -1)
    {
        this.isInit = false;
        this.readyToRemove = false;
        this.forceContinue = false;

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

    public virtual void PushActions(List<Action> actionsToPush)
    {
        linkedActions.AddRange(actionsToPush);

        for (int i = (actionsToPush.Count) - 1; i > 0; i--)
        {
            actionsToPush[i].linkedActions.AddRange(linkedActions);
            GM.actionManager.PushAction(actionId, actionsToPush[i]);
        }
    }

    public virtual void PushAction(Action actionToPush)
    {
        linkedActions.Add(actionToPush);
        actionToPush.linkedActions.AddRange(linkedActions);

        GM.actionManager.PushAction(actionId, actionToPush);
    }

    public virtual bool PlayersAreReady(bool log = false)
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
            linkedAnimations.Add(anim);
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

    public abstract void Execute();

    public virtual void OnComplete()
    {
        if (effectOrigin != 0 && cardOrigin != 0)
        {
            PlayerHolder player = GM.getPlayerHolder(photonId);
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