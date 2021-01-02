using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Gladiador/Red de Gladiador")]
public class CE_RedGladiador : SelectionCardEffect
{
    public override void Execute()
    {
        base.Execute();

        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        List<Card> available = opponent.playedCards.FindAll(a => (a.HasTags(CardTag.ATAQUE_BASICO) || a.HasTags(CardTag.GOLPE_CRITICO)) && !a.isBroken);

        if(available.Count > 0)
        {
            if (available.Count == 1)
            {
                Card card = available.First();
                card.BreakeCard();
            }
            else
            {
                if (card.owner.isLocal)
                {
                    parentAction.PushAction(new A_CardSelection("<b>Anula</b> un Ataque Basico o Golpe Critico", available, card.owner.photonId, this, card.instanceId));
                }
                else
                {
                    GM.actionManager.PushAction(parentAction.actionId, new A_CardSelectionWait(GM.localPlayer.photonId, this, card.instanceId));
                }
            }
        }
        else
        {
            Finish();
        }
    }

    public override void DoneSelecting(int[] cardIds)
    {
        if(cardIds != null)
        {
            Card card = GM.GetCard(cardIds.First());
            card.BreakeCard();
        }

        Finish();
    }
}
