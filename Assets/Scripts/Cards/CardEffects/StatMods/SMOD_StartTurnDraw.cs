﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMOD_StartTurnDraw : StatModification
{
    public SMOD_StartTurnDraw(int amount, bool isTemporary = true, int identifier = -1) : base(amount, isTemporary, identifier)
    {
        stat_mod = StatType.START_DRAW_AMOUNT;
    }

    public override int modify(int value)
    {
        return value += this.amount;
    }
}
