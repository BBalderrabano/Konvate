﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Pestilente/Antidoto")]
public class CE_Antidoto : CardEffect
{
    public int amountToPlace;
    private PlayerHolder enemy;
    bool placedChips = false;

    public override void Execute()
    {
        placedChips = false;

        enemy = GM.GetOpponentHolder(card.owner.photonId);

        List<Transform> enemyPoisonChips = GM.GetChips(ChipType.POISON, enemy, true);

        int playedAmount = enemyPoisonChips.Count;

        if (playedAmount == 0)
        {
            base.Finish();
            return;
        }

        else if (playedAmount > 0)
        {
            parentAction.LinkAnimation(GM.animationManager.RemoveChip(parentAction.actionId, this, card.owner.photonId, card.instanceId, ChipType.POISON, playedAmount, false));
        }
    }

    public override void Finish()
    {
        if (!placedChips)
        {
            parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.COMBAT, amountToPlace));

            placedChips = true;
        }
        else
        {
            base.Finish();
        }
    }
}
