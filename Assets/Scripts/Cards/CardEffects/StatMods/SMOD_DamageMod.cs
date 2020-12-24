using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMOD_DamageMod : StatModification
{
    public SMOD_DamageMod(int amount = 0, bool isTemporary = true) : base(amount, isTemporary)
    {
        stat_mod = StatType.DAMAGE_MODIFIER;
    }

    public override int modify(int value)
    {
        return 0;
    }
}
