using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Caballero/Refrigerio restore effect")]
public class CE_RefrigerioRestoreEffect : CardEffect
{
    private PlayerHolder enemy;

    public override void Execute()
    {
        base.Execute();

        enemy = GameManager.singleton.getOpponentHolder(card.owner.photonId);

        List<Transform> bloodChips = GameManager.singleton.GetChips(ChipType.BLEED, enemy, true);

        if(bloodChips.Count == 0)
        {
            Debug.Log(card.photonId + " recupera 3 " + card.instanceId);
            card.owner.ModifyBloodChip(3);
            isDone = true;
        }
        else
        {
            Debug.Log(card.photonId + " recibio danio este turno " + card.instanceId);
            isDone = true;
        }
    }
}
