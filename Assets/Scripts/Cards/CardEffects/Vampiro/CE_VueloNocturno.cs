using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Vampiro/Vuelo Nocturno")]
public class CE_VueloNocturno : CardEffect
{
    public int amount = 1;

    public override void Execute()
    {
        skipsEffectPreviewTime = false;
        skipsCardPreview = false;

        base.Execute();

        if(card.owner.playedCards.Exists(a => a.HasTags(new CardTags[] { CardTags.SHAPE })))
        {
            parentAction.LinkAnimation(GM.animationManager.RemoveChip(parentAction.actionId, this, card.owner.photonId, card.instanceId, ChipType.BLEED, amount));
        }
        else
        {
            skipsCardPreview = true;
            skipsEffectPreviewTime = true;
            Finish();
        }
    }
}
