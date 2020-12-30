using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Nigromante/Apocalipsis Zombie")]
public class CE_ApocalipsisZombie : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        int amount = opponent.playedCards.FindAll(a => a.GetCardType() is NormalPlay).Count;

        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.COMBAT, amount));
    }
}
