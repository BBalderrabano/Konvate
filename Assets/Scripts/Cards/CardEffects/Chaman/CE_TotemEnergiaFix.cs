using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Chaman/Totem Energia Fix")]
public class CE_TotemEnergiaFix : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        foreach (CE_TotemEnergiaBuff eff in card.cardEffects.OfType<CE_TotemEnergiaBuff>())
        {
            eff.isActive = true;
        }
    }
}
