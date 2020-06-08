using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Pestilente/Tolerancia")]
public class CE_Tolerancia : CardEffect
{
    int amount;
    int inPlayAmount;

    private PlayerHolder enemy;

    public override void Execute()
    {
        amount = card.owner.currentHolder.playedPoisonChipHolder.value.childCount;

        inPlayAmount = 0;

        enemy = GM.getOpponentHolder(card.owner.photonId);
        List<Transform> fistChips = GM.GetChips(ChipType.COMBAT_OFFENSIVE, enemy, true);

        if (fistChips.Count == 0 || amount == 0)
        {
            base.Finish();
            return;
        }

        if (amount > fistChips.Count)
        {
            inPlayAmount = fistChips.Count;
        }
        else
        {
            inPlayAmount = amount;
        }

        parentAction.LinkAnimation(GM.animationManager.RemoveChip(parentAction.actionId, this, card.owner.photonId, card.instanceId, ChipType.POISON, inPlayAmount));
    }
}
