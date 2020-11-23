using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Stat Mods/Start Turn Draw")]
public class SMOD_StartTurnDraw : StatModification
{
    public override int modify(int value)
    {
        return this.amount += value;
    }
}
