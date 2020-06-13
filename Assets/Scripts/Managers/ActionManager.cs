using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

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

    public void AddAction(Action action)
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

    public Action GetAction(int actionId)
    {
        foreach (ActionList list in actions)
        {
            foreach (Action action in list)
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

    public List<Action> GetPlayerActions(int photonId)
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

    public void PushAction(int actionId, Action action)
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

    public void Tick(float d)
    {
        foreach (ActionList actionList in actions)
        {
            for (int i = 0; i < actionList.Count; i++)
            {
                Action current = actionList[i];

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

public class ActionList : List<Action> {

    public int photonId;

    public ActionList(int photonId)
    {
        this.photonId = photonId;
    }
}
