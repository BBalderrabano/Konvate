using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Chaman/Totem de Energia Fix")]
public class CE_TotemEnergiaFix : CardEffect
{
    public override void Execute()
    {
        Debug.Log("ENTRo");

        card.cardEffects.OfType<CE_TotemEnergia>().First().isActive = true;

        card.MakeBorderActive();
    }
}
