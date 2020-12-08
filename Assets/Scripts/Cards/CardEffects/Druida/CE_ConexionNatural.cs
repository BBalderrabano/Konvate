using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Druida/Conexion Natural")]
public class CE_ConexionNatural : SelectionCardEffect
{
    public override void Execute()
    {
        base.Execute();

        List<Card> shapes = new List<Card>();

        shapes.AddRange(card.owner.deck.FindAll(a => a.HasTags(new CardTags[] { CardTags.SHAPE }.ToArray())));
        shapes.AddRange(card.owner.discardCards.FindAll(a => a.HasTags(new CardTags[] { CardTags.SHAPE }.ToArray())));

        if(shapes.Count > 0)
        {
            if (card.owner.isLocal)
            {
                parentAction.PushAction(new A_CardSelection("Coloca una <b>forma</b> en tu mano", shapes, card.owner.photonId, this, card.instanceId));
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
            Card selected = GM.GetCard(cardIds.First());

            card.owner.discardCards.Remove(selected);
            card.owner.deck.Remove(selected);

            card.owner.handCards.Add(selected);

            if (card.owner.isLocal)
            {
                selected.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.handLogic);
            }
            else
            {
                selected.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.opponentHandLogic);
            }

            AudioManager.singleton.Play(SoundEffectType.DRAW_CARD);

            parentAction.LinkAnimation(GM.animationManager.MoveCard(parentAction.actionId, card.owner.photonId, selected.instanceId, card.owner.currentHolder.handGrid.value.position, card.owner.currentHolder.handGrid.value.gameObject));
        }

        Finish();
    }
}
