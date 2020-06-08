using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Pestilente/Acelerar Toxina Place")]
public class CE_AcelerarToxina_Place : CardEffect
{
    [System.NonSerialized]
    public int amount;

    int temp_amount;

    public override void Execute()
    {
        base.Execute();

        PlayerHolder player = card.owner;

        temp_amount = amount;

        List<Transform> fistChips = GM.GetChips(ChipType.COMBAT, player);

        if (fistChips.Count == 0 || amount == 0)
        {
            base.Finish();
            return;
        }

        if (amount > fistChips.Count)
        {
            temp_amount = fistChips.Count;
        }

        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, player.photonId, card.instanceId, ChipType.COMBAT, temp_amount));
    }
}
