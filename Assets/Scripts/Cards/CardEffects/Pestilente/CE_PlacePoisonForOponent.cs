using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Pestilente/Explosion Ve Poison Chip")]
public class CE_PlacePoisonForOponent : CardEffect
{
    private PlayerHolder player;

    public override void Execute()
    {
        base.Execute();

        player = GameManager.singleton.getOpponentHolder(card.owner.photonId);

        List<Transform> poisonChip = GameManager.singleton.GetChips(ChipType.POISON, player);

        if(poisonChip.Count > 0)
        {
            parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, player.photonId, card.instanceId, ChipType.COMBAT));
        }
        else
        {
            base.Finish();
            return;
        }
    }
}
