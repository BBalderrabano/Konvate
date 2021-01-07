using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Duelista/Estocada")]
public class CE_Estocada : CardEffect
{
    public override void Execute()
    {
        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        if (GM.turn.turnFlags.GetFlag(card.owner.photonId, FlagDesc.INFLICTED_BLEED).amount > 0)
        {
            if (IsCombo())
            {
                opponent.statMods.Add(new SMOD_StartTurnDraw(-3, true));
            }
            else
            {
                if(!GM.turn.comboTracker.Exists(a => a.card_name == card.cardName && a.photonId == card.owner.photonId))
                {
                    opponent.statMods.Add(new SMOD_StartTurnDraw(-1, true));
                }
            }
        }
    }
}
