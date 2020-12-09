using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Arquero/Disparo Multiple Combo")]
public class CE_DisparoMultipleCombo : CardEffect
{
    public Card arrowTarget;

    public override void Execute()
    {
        base.Execute();

        if (IsCombo())
        {
            foreach(Card arrow in card.owner.discardCards.FindAll(a => a.cardName == arrowTarget.cardName)) 
            {
                card.owner.discardCards.Remove(arrow);
                card.owner.handCards.Add(arrow);

                if (card.owner.isLocal)
                {
                    arrow.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.handLogic);
                }
                else
                {
                    arrow.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.opponentHandLogic);
                }

                parentAction.LinkAnimation(GM.animationManager.MoveCard(parentAction.actionId, card.owner.photonId, arrow.instanceId, card.owner.currentHolder.handGrid.value.position, card.owner.currentHolder.handGrid.value.gameObject));

                AudioManager.singleton.Play(SoundEffectType.DRAW_CARD);
            }
        }
    }
}
