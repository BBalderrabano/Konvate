using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Pestilente/Acelerar Toxina Place")]
public class CE_AcelerarToxina_Place : CardEffect
{
    [System.NonSerialized]
    public int amount;

    public override void Execute()
    {
        base.Execute();

        if(amount > 0)
        {
            parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.COMBAT, amount));
        }
        else
        {
            base.Finish();
            return;
        }
    }
}
