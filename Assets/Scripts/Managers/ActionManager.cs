using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionManager
{
    List<ActionList> actions = new List<ActionList>();

    GameManager GM
    {
        get { return GameManager.singleton; }
    }

    public bool IsDone()
    {
        bool isDone = true;

        foreach (ActionList actionList in actions)
        {
            if (actionList.Count > 0)
            {
                isDone = false;
                break;
            }

        }
        
        return isDone;
    }

    public void AddAction(KAction action)
    {
        foreach (ActionList list in actions)
        {
            if(list.photonId == action.photonId)
            {
                list.Add(action);
                break;
            }
        }
    }

    public KAction GetAction(int actionId)
    {
        foreach (ActionList list in actions)
        {
            foreach (KAction action in list)
            {
                if (action.actionId == actionId)
                    return action;

                if(action.GetLinkedAction(actionId) != null)
                {
                    return action.GetLinkedAction(actionId);
                }
            }
        }

        return null;
    }

    public KAction GetActionByEffect(int effectId)
    {
        foreach (ActionList list in actions)
        {
            foreach (A_ExecuteEffect action in list.OfType<A_ExecuteEffect>())
            {
                if (action.effect.effectId == effectId)
                    return action;
            }
        }

        return null;
    }

    public List<KAction> GetPlayerActions(int photonId)
    {
        foreach (ActionList list in actions)
        {
            if (list.photonId == photonId)
            {
                return list;
            }
        }
        return null;
    }

    public void PushActionToStart(KAction action)
    {
        foreach (ActionList list in actions)
        {
            if (list.photonId == action.photonId)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].actionId == current.actionId)
                    {
                        list.Insert(i+1, action);
                        break;
                    }
                }
            }
        }
    }

    public void PushAction(int actionId, KAction action)
    {
        foreach (ActionList list in actions)
        {
            if (list.photonId == action.photonId)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].actionId == actionId)
                    {
                        list.Insert(i, action);
                        break;
                    }
                }
            }
        }
    }

    KAction current;

    public void Tick(float d)
    {
        foreach (ActionList actionList in actions)
        {
            for (int i = 0; i < actionList.Count; i++)
            {
                current = actionList[i];

                if (!current.IsComplete())
                {
                    current.Execute(d);

                    if (!current.Continue())
                    {
                        break;
                    }
                }
                else
                {
                    if (current.LinkedActionsReady() && !current.readyToRemove)
                    {
                        current.OnComplete();
                    }
                }
            }

            actionList.RemoveAll(a => a.readyToRemove);
        }
    }

    public ActionManager()
    {
        actions.Clear();

        for (int i = 0; i < GM.allPlayers.Length; i++)
        {
            actions.Add(new ActionList(GM.allPlayers[i].photonId));
        }
    }
}

public class ActionList : List<KAction> {

    public int photonId;

    public ActionList(int photonId)
    {
        this.photonId = photonId;
    }
}
