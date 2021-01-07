using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnFlags
{
    public List<TurnFlag> flags;

    public TurnFlags()
    {
        flags = new List<TurnFlag>();
    }

    public void RemoveTemporary()
    {
        flags.RemoveAll(a => a.isTemporary);
    }

    public void ResetFlags()
    {
        flags.Clear();
    }

    public void AddFlag(TurnFlag flag)
    {
        bool found = false;

        for (int i = 0; i < flags.Count; i++)
        {
            if (flags[i].photonId == flag.photonId && flags[i].desc == flag.desc)
            {
                flags[i].amount += flag.amount;
                found = true;
                break;
            }
        }

        if (!found)
        {
            flags.Add(flag);
        }
    }

    public TurnFlag GetFlag(int photonId, FlagDesc desc)
    {
        for (int i = 0; i < flags.Count; i++)
        {
            if(flags[i].photonId == photonId && flags[i].desc == desc)
            {
                return flags[i];
            }
        }

        return new TurnFlag(photonId, desc, 0);
    }
}

public class TurnFlag
{
    public int photonId;
    public FlagDesc desc;
    public int amount;
    public bool isTemporary;

    public TurnFlag(int photonId, FlagDesc desc, int amount, bool isTemporary = true)
    {
        this.photonId = photonId;
        this.desc = desc;
        this.amount = amount;
        this.isTemporary = isTemporary;
    }
}

public enum FlagDesc {
    INFLICTED_BLEED,
    DISCARD_AMOUNT_BY_EFF
}