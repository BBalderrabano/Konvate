using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Brujo/Curse Place Poison Chip")]
public class CE_CursePlacePoison : CardEffect
{
    public int amount = 1;

    public override void Execute()
    {
        base.Execute();
        
        if(card.owner.photonId != card.photonId)
        {
            skipsEffectPreviewTime = false;    
            parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.photonId, card.instanceId, ChipType.POISON, amount));
        }
        else
        {
            skipsEffectPreviewTime = true;
        }
    }
}
