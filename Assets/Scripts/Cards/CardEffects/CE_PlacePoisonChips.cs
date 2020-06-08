using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Place Poison Chip")]
public class CE_PlacePoisonChips : CardEffect
{
    public int amount;

    private PlayerHolder player;

    public override void Execute()
    {
        base.Execute();

        player = card.owner;

        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, player.photonId, card.instanceId, ChipType.POISON, amount));
    }
}
