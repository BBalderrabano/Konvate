using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Vampiro/Elegancia Vampirica Transfer")]
public class CE_EleganciaVampiricaTransfer : CardEffect
{
    [System.NonSerialized]
    public PlayerHolder new_owner;

    public override void Execute()
    {
        parentAction.MakeActiveOnComplete(false);
        skipsEffectPreviewTime = true;
        skipsCardPreview = true;

        PlayerHolder owner = card.owner;

        owner.handCards.Remove(card);
        owner.playedCards.Remove(card);
        owner.discardCards.Remove(card);
        owner.deck.Remove(card);
        owner.all_cards.Remove(card);

        card.owner = new_owner;

        card.cardPhysicalInst.transform.SetParent(new_owner.currentHolder.discardGrid.value);
        card.cardPhysicalInst.transform.position = new_owner.currentHolder.discardGrid.value.position;

        new_owner.discardCards.Add(card);

        new_owner.all_cards.Add(card);
    }

    public override void Finish()
    {
        base.Finish();

        card.cardEffects.Remove(this);
    }
}
