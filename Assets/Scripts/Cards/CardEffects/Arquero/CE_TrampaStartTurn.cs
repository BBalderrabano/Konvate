using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Arquero/Trampa Reduccion")]
public class CE_TrampaStartTurn : CardEffect
{
    [System.NonSerialized]
    public bool isActive = false;

    SMOD_StartTurnDraw startTurnPenalty = new SMOD_StartTurnDraw(-1);

    public override void Execute()
    {
        skipsEffectPreviewTime = !isActive;

        if (isActive)
        {
            PlayerHolder enemy = GM.GetOpponentHolder(card.owner.photonId);

            enemy.statMods.Add(startTurnPenalty);
        }
        else
        {
            GM.HidePreviewCard();
        }
    }
}
