using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Orco/Frenesi Stat Mod")]
public class CE_Frenesi_SMODS : CardEffect
{
    public SMOD_StartTurnDraw temp_draw;
    public SMOD_StartTurnEnergy temp_energy;

    public override void Execute()
    {
        skipsEffectPreview = false;

        if (GM.turn.playerInflictedBleed(card.owner.photonId))
        {
            card.cardEffects.Add((CardEffect)temp_draw.Clone());
            card.cardEffects.Add((CardEffect)temp_energy.Clone());
        }
        else
        {
            skipsEffectPreview = true;
            GM.HidePreviewCard();
        }
    }
}
