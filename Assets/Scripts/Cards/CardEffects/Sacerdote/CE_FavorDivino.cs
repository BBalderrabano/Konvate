using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Sacerdote/Favor Divino")]
public class CE_FavorDivino : CardEffect
{
    public int amount = 4;

    public override void Execute()
    {
        base.Execute();

        card.owner.ModifyHitPoints(amount);

        card.owner.deck.Remove(card);
        card.owner.discardCards.Remove(card);
        card.owner.handCards.Remove(card);
        card.owner.playedCards.Remove(card);
        card.owner.all_cards.Remove(card);

        LeanTween.scale(card.cardPhysicalInst.gameObject, Vector3.zero, Settings.ANIMATION_TIME).setOnComplete(() =>
        {
            card.cardPhysicalInst.gameObject.SetActive(false);
        });
    }
}
