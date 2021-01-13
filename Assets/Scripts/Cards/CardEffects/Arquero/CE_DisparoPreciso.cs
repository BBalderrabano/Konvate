using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Arquero/Disparo Preciso")]
public class CE_DisparoPreciso : CardEffect
{
    public int amount = 2;

    public override void Execute()
    {
        base.Execute();

        PlayerHolder enemy = GM.GetOpponentHolder(card.owner.photonId);

        skipsEffectPreviewTime = false;

        if (!enemy.playedCards.Any(a => a.HasTags(CardTag.DEFENSA)) && !enemy.playedCards.Any(a => a.HasTags(CardTag.DEFENSA_SUPERIOR)))
        {
            parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.COMBAT, amount));
        }
        else
        {
            skipsEffectPreviewTime = true;
        }
    }
}
