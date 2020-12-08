using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Druida/Defensa Superior")]
public class CE_Druida_DefensaSuperior : CardEffect
{
    PlayerHolder player;

    public int amount = 1;
    public int bear_amount = 1;

    public override void Execute()
    {
        base.Execute();

        player = card.owner;

        int n = amount;

        if(player.playedCards.Find(a => a.HasTags(new CardTags[] { CardTags.SHAPE_BEAR })))
        {
            n += bear_amount;
        }

        parentAction.LinkAnimation(GM.animationManager.RemoveChip(parentAction.actionId, this, player.photonId, card.instanceId, ChipType.COMBAT_OFFENSIVE, n));
    }
}
