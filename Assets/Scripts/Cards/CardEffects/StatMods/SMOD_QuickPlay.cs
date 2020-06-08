using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Card Effects/Stat Mods/Quick Play")]
public class SMOD_QuickPlay : StatModification
{
    public override int modify(int value)
    {
        return -1;
    }
}
