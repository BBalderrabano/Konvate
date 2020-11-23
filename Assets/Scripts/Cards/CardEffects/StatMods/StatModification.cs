using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatModification : CardEffect
{
    public StatType stat_mod;

    public int amount;

    public abstract int modify(int value);
}

public enum StatType
{
    ENERGY_COST,
    START_DRAW_AMOUNT,
    QUICK_PLAY,
    START_ENERGY_AMOUNT
}