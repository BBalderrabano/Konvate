using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Arquero/Trampa")]
public class CE_Trampa : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        PlayerHolder enemy = GM.GetOpponentHolder(card.owner.photonId);

        if (enemy.playedCards.Any(a => a.HasTags(CardTag.ATAQUE_BASICO)) || enemy.playedCards.Any(a => a.HasTags(CardTag.GOLPE_CRITICO)))
        {
            card.cardEffects.OfType<CE_TrampaStartTurn>().First().isActive = true;
        }
        else
        {
            card.cardEffects.OfType<CE_TrampaStartTurn>().First().isActive = false;
        }

        skipsEffectPreviewTime = true;
        GM.HidePreviewCard();
    }
}
