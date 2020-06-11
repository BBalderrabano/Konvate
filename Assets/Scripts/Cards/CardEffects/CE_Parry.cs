using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Parry")]
public class CE_Parry : CardEffect
{
    public int amount;
    private PlayerHolder enemy;

    public override void Execute()
    {
        base.Execute();

        enemy = GameManager.singleton.getOpponentHolder(card.owner.photonId);

        List<Transform> fistChips = GameManager.singleton.GetChips(ChipType.COMBAT_OFFENSIVE, enemy, true);

        if (fistChips.Count > 0)
        {
            parentAction.LinkAnimation(
                GM.animationManager.MoveChip(
                    fistChips[0].gameObject, 
                    parentAction.actionId, 
                    card.owner.photonId, 
                    card.owner.currentHolder.playedCombatChipHolder.value.position, 
                    card.owner.currentHolder.playedCombatChipHolder.value.gameObject));
        }
        else
        {
            base.Finish();
        }
    }
}
