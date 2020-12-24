using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Samurai/Ataque Final")]
public class CE_AtaqueFinal : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        int amount = Mathf.Max(0, card.owner.maxHealth - card.owner.bleedCount);
        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        GM.DrawCard(opponent, 1, card.instanceId, effectId, parentAction);
        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.COMBAT, amount));
    }
}
