using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Pestilente/Explosion Ve Poison Chip")]
public class CE_PlacePoisonForOponent : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        PlayerHolder enemy = GM.GetOpponentHolder(card.owner.photonId);

        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, enemy.photonId, card.instanceId, ChipType.POISON));
    }
}
