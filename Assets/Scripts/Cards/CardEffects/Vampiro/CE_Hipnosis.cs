using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Vampiro/Hipnosis Selection")]
public class CE_Hipnosis : SelectionCardEffect
{
    public Vampiro_CD_Hipnosis condition;

    public override void Execute()
    {
        base.Execute();

        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        if(opponent.handCards.Count == 0)
        {
            Finish();
        }
        else
        {
            if (card.owner.isLocal)
            {
                parentAction.PushAction(new A_CardSelection("Tu oponente debe jugar esta carta <b>primero</b>", opponent.handCards, card.owner.photonId, this, card.instanceId));
            }
            else
            {
                GM.actionManager.PushAction(parentAction.actionId, new A_CardSelectionWait(GM.localPlayer.photonId, this, card.instanceId));
            }
        }
    }

    public override void DoneSelecting(int[] cardIds)
    {
        if (cardIds != null)
        {
            PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

            Card c = GM.GetCard(cardIds.First());

            Vampiro_CD_Hipnosis cd_clone = (Vampiro_CD_Hipnosis)condition.Clone();

            cd_clone.mustPlayCard = c;

            for (int i = 0; i < opponent.all_cards.Count; i++)
            {
                if (opponent.all_cards[i] == c)
                    continue;

                opponent.all_cards[i].conditions.Add(cd_clone);
            }
        }
    }
}
