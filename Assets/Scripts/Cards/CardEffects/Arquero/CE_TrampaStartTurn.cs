using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Arquero/Trampa Reduccion")]
public class CE_TrampaStartTurn : CardEffect
{
    [System.NonSerialized]
    public bool isActive = false;

    public SMOD_StartTurnDraw startTurnPenalty;

    public override void Execute()
    {
        skipsEffectPreview = !isActive;

        if (isActive)
        {
            PlayerHolder enemy = GM.GetOpponentHolder(card.owner.photonId);

            enemy.statModifications.Add(startTurnPenalty);
        }
        else
        {
            GM.HidePreviewCard();
        }
    }
}
