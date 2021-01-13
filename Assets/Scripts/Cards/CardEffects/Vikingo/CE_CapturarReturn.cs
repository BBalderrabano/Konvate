using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Vikingo/Capturar Return")]
public class CE_CapturarReturn : CardEffect
{
    [System.NonSerialized]
    public Card captureCard;

    public CE_CapturarTransfer transferEffect;

    public override void Execute()
    {
        skipsCardPreview = true;
        isAnimatingLeanTween = true;

        parentAction.MakeActiveOnComplete(false);

        GM.PreviewCard(captureCard);

        PlayerHolder original_owner = GM.GetPlayerHolder(card.photonId);

        transferEffect.new_owner = original_owner;

        CardEffect transfer = transferEffect.Clone(card, false);

        card.MakeBorderInactive();
        card.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.deckLogic);
        card.cardEffects.Add(transfer);
        card.owner.playedCards.Remove(card);

        card.cardViz.cardFrontImage.color = Color.white;

        LeanTween.move(card.cardPhysicalInst.gameObject, original_owner.currentHolder.deckGrid.value.position, Settings.ANIMATION_TIME)
        .setEaseInOutQuad()
        .setDelay(Settings.ANIMATION_DELAY)
        .setOnComplete(() => {
            foreach (CardEffect eff in card.cardEffects)
            {
                if (eff.type == EffectType.STARTTURN && eff != this)
                {
                    GM.turn.startTurnEffects.Add(eff);
                }
            }

            isAnimatingLeanTween = false;
        });

        ////////////////////////////////////////////CAPTURE CARD////////////////////////////////////////////////////////////////////////

        Vector3 original_scale = captureCard.cardPhysicalInst.transform.localScale;

        LeanTween.scale(captureCard.cardPhysicalInst.gameObject, original_scale, Settings.ANIMATION_TIME).setDelay(Settings.ANIMATION_DELAY).setFrom(Vector3.zero);

        captureCard.owner.deck.Insert(Random.Range(0, captureCard.owner.deck.Count), captureCard);

        captureCard.owner.all_cards.Add(captureCard);

        captureCard.MakeBorderInactive();
        captureCard.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.deckLogic);

        captureCard.cardPhysicalInst.gameObject.SetActive(true);
    }
}
