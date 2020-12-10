using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Chaman/Totem rompible")]
public class CE_TotemBreake : SelectionCardEffect
{
    public int amount_required = 1;

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
            List<Card> totem = new List<Card>
            {
                card
            };

            parentAction.PushAction(new A_CardSelection("¿Usar "+amount_required+" <sprite=0>?", totem, enemy.photonId, this, card.instanceId).ModifyParameters(true, false, 0, 0));
        }
    }

    public override void DoneSelecting(int[] cardIds)
    {
        if (cardIds == null)
        {
            Debug.Log("Dijo que no");
        }
        else
        {
            Debug.Log("Dijo que si");
        }


    }
}
