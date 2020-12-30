using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Nigromante/Explosion de Cadaveres Place")]
public class CE_ExplosionCadaveresPlace : CardEffect
{
    public Card zombie_card;

    public override void Execute()
    {
        base.Execute();

        int amount = card.owner.discardCards.FindAll(a => a.cardName == zombie_card.cardName).Count;

        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.POISON, Mathf.Max(1, amount)));
    }
}
