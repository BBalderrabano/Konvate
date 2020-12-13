using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Areas/PlayAreaWhenHoldingCard")]
public class PlayCardAreaLogic : AreaLogic
{
    public CardVariable card;

    public override void Execute(Vector3 position)
    {
        if (ScrollSelectionManager.singleton.isActive)
            return;

        if (card.value == null)
            return;

        GameManager GM = GameManager.singleton;
        PlayerHolder owner = card.value.viz.card.owner;

        if (!GM.currentPlayer.isLocal) {
            WarningPanel.singleton.ShowWarning("No es tu turno");
            return;
        }

        Card c = owner.GetCard(card.value.viz.card.instanceId);

        bool canUseCard = owner.CanUseCard(c);

        if (canUseCard && GM.turn.currentPhase.value.CanPlayCard(c) && ConditionsAreMet(c)) 
        {
            KAction playCard = new A_PlayCard(c.instanceId, GM.localPlayer.photonId);
            GM.actionManager.AddAction(playCard);
        }
    }

    bool ConditionsAreMet(Card c)
    {
        bool conditionsMet;

        for (int i = 0; i < c.conditions.Count; i++)
        {
            conditionsMet = c.conditions[i].isValid(c.owner.photonId, c.instanceId);

            if (!conditionsMet)
            {
                return false;
            }
        }

        return true;
    }

}
