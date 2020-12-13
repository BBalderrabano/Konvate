using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Logics/Set Card")]
public class SetCard : CardLogic
{
    public CardLogic cardInHandLogic;
    public PlayerHolder localPlayer;

    public override void OnClick(CardInstance inst)
    {
        if (ScrollSelectionManager.singleton.isActive)
            return;

        if (GameManager.singleton.turn.currentPhase.value is SetCardsPhase)
        {
            if (inst.viz.card.owner.isLocal)
            { 
                Card card = inst.viz.card;

                if (card.owner.hasCardOnLocation(card.instanceId, PlayerHolder.CardLocation.Play))
                {
                    KAction returnToHand = new A_ReturnToHand(card.instanceId, card.owner.photonId);
                    GameManager.singleton.actionManager.AddAction(returnToHand);
                }
            }
        }
    }

    public override void OnHighlight(CardInstance inst)
    {
    }

    public override void OnSetLogic(CardInstance inst)
    {
        if (!inst.viz.card.owner.isLocal)
        {
            inst.viz.cardBackImage.gameObject.SetActive(true);
        }
    }
}