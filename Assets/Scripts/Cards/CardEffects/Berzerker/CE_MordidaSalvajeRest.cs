using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Berzerker/Mordida Salvaje Restore")]
public class CE_MordidaSalvajeRest : CardEffect
{
    public int amount = 1;

    public override void Execute()
    {
        base.Execute();

        if (GM.turn.playerInflictedBleed(card.owner.photonId))
        {
            card.owner.ModifyHitPoints(amount);
        }
    }
}
