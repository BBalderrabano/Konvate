using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Berzerker/Mordida Salvaje Restore")]
public class CE_MordidaSalvajeRest : CardEffect
{
    public int amount = 1;

    public override void Execute()
    {
        base.Execute();

        if (GM.turn.turnFlags.GetFlag(card.owner.photonId, FlagDesc.INFLICTED_BLEED).amount > 0)
        {
            card.owner.ModifyHitPoints(amount);
        }
    }
}
