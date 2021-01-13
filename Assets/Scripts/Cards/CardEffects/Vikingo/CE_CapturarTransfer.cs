using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Vikingo/Capturar Transfer")]
public class CE_CapturarTransfer : CardEffect
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

        card.cardPhysicalInst.transform.position = new_owner.currentHolder.deckGrid.value.position;

        new_owner.deck.Insert(Random.Range(0, new_owner.deck.Count), card);

        new_owner.all_cards.Add(card);
    }

    public override void Finish()
    {
        base.Finish();

        card.cardEffects.Remove(this);
    }
}
