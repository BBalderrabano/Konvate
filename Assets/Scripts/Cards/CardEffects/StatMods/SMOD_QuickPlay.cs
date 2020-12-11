using UnityEngine;
using System.Collections;

public class SMOD_QuickPlay : StatModification
{
    public SMOD_QuickPlay(int amount = 0, bool isTemporary = true) : base(amount, isTemporary)
    {
        stat_mod = StatType.QUICK_PLAY;
    }

    public override int modify(int value)
    {
        return -1;
    }
}
