using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Pestilente/Acelerar Toxina Remove")]
public class CE_AcelerarToxina_Remove : CardEffect
{
    private PlayerHolder enemy;

    public override void Execute()
    {
        int totalAmount = 3;

        enemy = GameManager.singleton.getOpponentHolder(card.owner.photonId);

        List<Transform> enemyPoisonChips = GameManager.singleton.GetChips(ChipType.POISON, enemy, true);
        List<Transform> playerPoisonChips = GameManager.singleton.GetChips(ChipType.POISON, card.owner, true);

        List<Transform> poisonChips = new List<Transform>();

        if(enemyPoisonChips.Count > 0)
        {
            poisonChips.AddRange(enemyPoisonChips.GetRange(0, Mathf.Min(enemyPoisonChips.Count, 3)));
        }

        if(playerPoisonChips.Count > 0 && poisonChips.Count < totalAmount)
        {
            poisonChips.AddRange(playerPoisonChips.GetRange(0, totalAmount - poisonChips.Count));
        }

        int playedAmount = poisonChips.Count;

        foreach (CE_AcelerarToxina_Place place in card.cardEffects.OfType<CE_AcelerarToxina_Place>())
        {
            place.amount = playedAmount;
        }

        if (playedAmount == 0)
        {
            base.Finish();
            return;
        }

        parentAction.LinkAnimation(GM.animationManager.RemoveChip(parentAction.actionId, this, card.owner.photonId, card.instanceId, ChipType.POISON, playedAmount));
    }
}
