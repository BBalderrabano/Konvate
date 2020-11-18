using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Orco/Odio Orco")]
public class CE_OdioOrco : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        card.owner.ModifyHitPoints(card.owner.currentHolder.playedCombatChipHolder.value.childCount);
    }
}
