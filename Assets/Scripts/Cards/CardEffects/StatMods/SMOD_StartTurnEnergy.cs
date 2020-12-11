using UnityEngine;

public class SMOD_StartTurnEnergy : StatModification
{
    public SMOD_StartTurnEnergy(int amount, bool isTemporary = true) : base(amount, isTemporary)
    {
        stat_mod = StatType.START_ENERGY_AMOUNT;
    }

    public override int modify(int value)
    {
        return value += this.amount;
    }
}
