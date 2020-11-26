using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Card Effects/Jinete/Lanza")]
public class CE_Lanza : CardEffect
{
    public int amount;

    private PlayerHolder player;

    public override void Execute()
    {
        player = card.owner;

        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, player.photonId, card.instanceId, ChipType.COMBAT, amount));
    }
}
