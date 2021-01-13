using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Orco/Frenesi Stat Mod")]
public class CE_Frenesi_SMODS : CardEffect
{
    SMOD_StartTurnDraw temp_draw = new SMOD_StartTurnDraw(1);
    SMOD_StartTurnEnergy temp_energy = new SMOD_StartTurnEnergy(1);

    public override void Execute()
    {
        skipsEffectPreviewTime = false;

        if (GM.turn.turnFlags.GetFlag(card.owner.photonId, FlagDesc.INFLICTED_BLEED).amount > 0)
        {
            card.owner.statMods.Add(temp_draw);
            card.owner.statMods.Add(temp_energy);
        }
        else
        {
            skipsEffectPreviewTime = true;
            GM.HidePreviewCard();
        }
    }
}
