using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Duelista/Estocada")]
public class CE_Estocada : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        if (GM.turn.playerInflictedBleed(card.owner.photonId))
        {
            if (IsCombo())
            {
                if (!opponent.statMods.Exists(a => a.identifier == effectId))
                {
                    opponent.statMods.Add(new SMOD_StartTurnDraw(-3, true, effectId));
                }
            }
            else
            {
                opponent.statMods.Add(new SMOD_StartTurnDraw(-1, true, effectId));
            }
        }
    }
}
