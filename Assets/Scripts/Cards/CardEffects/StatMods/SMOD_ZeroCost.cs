using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMOD_ZeroCost : StatModification
{
    public SMOD_ZeroCost(int amount = 0, bool isTemporary = true) : base(amount, isTemporary)
    {
        stat_mod = StatType.ENERGY_COST;
    }

    public override int modify(int value)
    {
        return 0;
    }
}
