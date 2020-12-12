using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Chaman/Encantar Arma")]
public class CE_EncantarArma : CardEffect
{
    public int amount = 1;

    public override void Execute()
    {
        base.Execute();

        foreach (Card c in card.owner.playedCards.Where(a => a.HasTags(CardTag.ATAQUE_BASICO)))
        {
            parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, c.instanceId, ChipType.COMBAT, amount));
        }
    }
}
