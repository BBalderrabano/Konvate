using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Card Effects/Gladiador/No estan entretenidos Energy")]
public class CE_NoEstanEntretenidosEnergy : CardEffect
{
    public override void Execute()
    {
        if (card.owner.playedCards.Contains(card))
        {
            base.Execute();

            skipsEffectPreviewTime = false;
            parentAction.MakeActiveOnComplete(true);

            foreach (PlayerHolder p in GM.allPlayers)
            {
                p.statMods.Add(new SMOD_StartTurnEnergy(1));
            }
        }
        else
        {
            skipsEffectPreviewTime = true;
            parentAction.MakeActiveOnComplete(false);
        }
    }
}
