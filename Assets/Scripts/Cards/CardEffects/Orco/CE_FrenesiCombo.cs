using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Orco/Frenesi Combo")]
public class CE_FrenesiCombo : CardEffect
{
    public int place_amount = 1;

    public override void Execute()
    {
        base.Execute();

        if (IsCombo())
        {
            parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.COMBAT, place_amount));
        }
    }

    public override void Finish()
    {
        base.Finish();

        card.MakeBorderActive();
    }
}
