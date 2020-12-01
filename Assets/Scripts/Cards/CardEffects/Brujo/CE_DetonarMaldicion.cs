using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Brujo/Detonar Maldicion")]
public class CE_DetonarMaldicion : CardEffect
{
    private PlayerHolder player;
    private PlayerHolder opponent;

    public override void Execute()
    {
        base.Execute();

        player = card.owner;
        opponent = GM.GetOpponentHolder(player.photonId);

        int amount = opponent.discardCards.Where(a => a.HasTags(new CardTags[] { CardTags.CURSE_BRUJO })).ToList().Count;

        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, player.photonId, card.instanceId, ChipType.COMBAT, amount));
    }
}
