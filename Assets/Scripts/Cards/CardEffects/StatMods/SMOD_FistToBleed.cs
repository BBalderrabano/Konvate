using UnityEngine;
using System.Collections;

public class SMOD_FistToBleed : StatModification
{
    public SMOD_FistToBleed(int amount = 0, bool isTemporary = true) : base(amount, isTemporary)
    {
        stat_mod = StatType.PREVENT_FIST_TO_BLEED;
    }

    public override int modify(int value)
    {
        return 0;
    }
}
