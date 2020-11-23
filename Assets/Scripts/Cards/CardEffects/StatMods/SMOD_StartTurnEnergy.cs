using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Stat Mods/Start Turn Energy")]
public class SMOD_StartTurnEnergy : StatModification
{
    public override int modify(int value)
    {
        return this.amount += value;
    }
}
