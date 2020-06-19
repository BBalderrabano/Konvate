using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Pestilente/Tolerancia")]
public class CE_Tolerancia : CardEffect
{
    int amount = 0;

    public override void Execute()
    {
        amount = card.owner.currentHolder.playedPoisonChipHolder.value.childCount;

        parentAction.LinkAnimation(GM.animationManager.RemoveChip(parentAction.actionId, this, card.owner.photonId, card.instanceId, ChipType.COMBAT_OFFENSIVE, amount));
    }
}
