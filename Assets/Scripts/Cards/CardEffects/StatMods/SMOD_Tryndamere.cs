using UnityEngine;
using System.Collections;

public class SMOD_Tryndamere : StatModification
{
    public SMOD_Tryndamere(int amount = 0, bool isTemporary = true) : base(amount, isTemporary)
    {
        stat_mod = StatType.TRYNDAMERE;
    }

    public override int modify(int value)
    {
        return 0;
    }
}
