using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Pestilente/Acelerar Toxina Remove")]
public class CE_AcelerarToxina_Remove : CardEffect
{
    private PlayerHolder enemy;

    public override void Execute()
    {
        base.Execute();

        int totalAmount = 3;
        int currentAmount = 0;

        enemy = GM.GetOpponentHolder(card.owner.photonId);

        List<Transform> enemyPoisonChips = GM.GetChips(ChipType.POISON, enemy, true);
        List<Transform> playerPoisonChips = GM.GetChips(ChipType.POISON, card.owner, true);

        if(enemyPoisonChips.Count > 0)
        {
            int enemyChipsAmount = Mathf.Min(enemyPoisonChips.Count, 3);

            currentAmount += enemyChipsAmount;

            parentAction.LinkAnimation(GM.animationManager.RemoveChip(parentAction.actionId, this, card.owner.photonId, card.instanceId, ChipType.POISON, enemyChipsAmount, false));
        }

        if(playerPoisonChips.Count > 0 && currentAmount < totalAmount)
        {
            int remainingChipsAmount = Mathf.Min(playerPoisonChips.Count, (totalAmount - currentAmount));

            currentAmount += remainingChipsAmount;

            parentAction.LinkAnimation(GM.animationManager.RemoveChip(parentAction.actionId, this, enemy.photonId, card.instanceId, ChipType.POISON, remainingChipsAmount, false));
        }

        foreach (CE_AcelerarToxina_Place place in card.cardEffects.OfType<CE_AcelerarToxina_Place>())
        {
            place.amount = currentAmount;
        }

        if (currentAmount == 0)
        {
            base.Finish();
            return;
        }
    }
}
