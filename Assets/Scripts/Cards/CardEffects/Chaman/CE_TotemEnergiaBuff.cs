using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Chaman/Totem Energia Buff")]
public class CE_TotemEnergiaBuff : CardEffect
{
    [System.NonSerialized]
    public bool isActive = false;

    public override void Execute()
    {
        skipsEffectPreview = !isActive;

        if (isActive)
        {
            card.owner.statMods.Add(new SMOD_StartTurnDraw(1));
            card.owner.statMods.Add(new SMOD_StartTurnEnergy(1));

            isActive = false;
        }

        card.MakeBorderInactive();
    }
}
