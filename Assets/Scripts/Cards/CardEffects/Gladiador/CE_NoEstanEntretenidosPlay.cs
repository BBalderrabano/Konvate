using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Gladiador/No estan entretenidos Play")]
public class CE_NoEstanEntretenidosPlay : CardEffect
{
    public override void Execute()
    {
        skipsEffectPreviewTime = true;

        foreach (CE_NoEstanEntrenidosPlace eff in card.cardEffects.OfType<CE_NoEstanEntrenidosPlace>().ToList())
        {
            eff.start_turn = GM.turn.turnCount;
        }
    }
}
