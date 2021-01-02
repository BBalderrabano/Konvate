using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Gladiador/No estan entretenidos Question")]
public class CE_NoEstanEntretenidosQuestion : SelectionCardEffect
{
    PlayerHolder enemy;

    public override void Execute()
    {
        enemy = GM.GetOpponentHolder(card.owner.photonId);

        if (card.owner.isLocal)
        {
            GM.actionManager.PushAction(parentAction.actionId, new A_CardSelectionWait(GM.localPlayer.photonId, this, card.instanceId, enemy.photonId));
        }
        else
        {
            List<Card> target = new List<Card>
            {
                card
            };

            parentAction.PushAction(new A_CardSelection("¿Iniciar el proximo turno con <sprite=4> menos?", target, enemy.photonId, this, card.instanceId).ModifyParameters(true, false, 0, 0));
        }
    }

    public override void DoneSelecting(int[] cardIds)
    {
        if(cardIds != null)
        {
            parentAction.PushAction(new A_Discard(cardIds.First(), card.owner.photonId, true));
            enemy.statMods.Add(new SMOD_StartTurnEnergy(-1).GiveProtection(1));
        }

        Finish();
    }
}
