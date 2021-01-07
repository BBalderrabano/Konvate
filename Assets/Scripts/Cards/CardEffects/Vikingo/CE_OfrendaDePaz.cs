using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Vikingo/Ofrenda de Paz")]
public class CE_OfrendaDePaz : SelectionCardEffect
{
    public override void Execute()
    {
        base.Execute();

        PlayerHolder enemy = GM.GetOpponentHolder(card.owner.photonId);

        if (card.owner.isLocal)
        {
            GM.actionManager.PushAction(parentAction.actionId, new A_CardSelectionWait(GM.localPlayer.photonId, this, card.instanceId, enemy.photonId));
        }
        else
        {
            parentAction.PushAction(new A_CardSelection("¿Aceptas la <b>tregua</b>?", card, enemy.photonId, this, card.instanceId).ModifyParameters(true, false, 0, 0));
        }
    }

    public override void DoneSelecting(int[] cardIds)
    {
        if (cardIds != null)
        {
            for (int i = 0; i < GM.allPlayers.Length; i++)
            {
                GM.allPlayers[i].statMods.Add(new SMOD_DamageMod());
            }
        }

        Finish();
    }
}
