using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Stat Mods/Zero Cost")]
public class SMOD_ZeroCost : StatModification
{
    public override int modify(int value)
    {
        return 0;
    }
}
