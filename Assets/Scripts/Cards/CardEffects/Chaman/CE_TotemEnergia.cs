using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Chaman/Totem de Energia")]
public class CE_TotemEnergia : CardEffect
{
    public SMOD_StartTurnDraw temp_draw;
    public SMOD_StartTurnEnergy temp_energy;

    [System.NonSerialized]
    public bool isActive = false;

    public override void Execute()
    {
        base.Execute();

        skipsEffectPreview = !isActive;

        if (isActive)
        {
            CardEffect clone = (CardEffect)temp_draw.Clone();

            card.cardEffects.Add((StatModification)temp_energy.Clone());
            card.cardEffects.Add(clone);

            isActive = false;
        }
        else
        {
            GM.HidePreviewCard();
        }
    }
}
