using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Conditions/Goblin/Chantaje")]
public class Goblin_CD_Chantaje : Condition
{
    public override bool isTemporary()
    {
        return false;
    }

    public override bool isValid(int photonId, int cardId = -1)
    {
        PlayerHolder player = GameManager.singleton.GetOpponentHolder(photonId);

        if(player.playedCards.FindAll(a => a.GetCardType() is QuickPlay && !a.EffectsDone() && !a.isBroken).Count < 1)
        {
            WarningPanel.singleton.ShowWarning("No hay carta para anular");

            return false;
        }

        return true;
    }
}
