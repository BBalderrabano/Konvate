using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Draw Card")]
public class CE_DrawCard : CardEffect
{
    public int amount;

    public override void Execute()
    {
        base.Execute();

        parentAction.PushActions(GM.DrawCard(card.owner, amount, card.instanceId, effectId, parentAction));
    }
}
