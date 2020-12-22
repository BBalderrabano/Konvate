using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Duelista/Tomar Ofensiva")]
public class CE_TomarOfesiva : SelectionCardEffect
{
    public override void Execute()
    {
        base.Execute();

        parentAction.MakeActiveOnComplete(true);

        if(GM.turn.offensivePlayer.photonId != card.owner.photonId)
        {
            GameObject offensive_chip = GM.turn.offensiveChip;
            Chip off_chip_component = offensive_chip.GetComponent<Chip>();

            GM.turn.offensivePlayer = card.owner;
            off_chip_component.owner = card.owner;
            off_chip_component.state = ChipSate.PLAYED;

            parentAction.LinkAnimation(GM.animationManager.MoveChip(offensive_chip, parentAction.actionId, card.owner.photonId, card.owner.currentHolder.playedCombatChipHolder.value.position,
                                                                                                                                card.owner.currentHolder.playedCombatChipHolder.value.gameObject));
        }

        if (card.owner.isLocal)
        {
            List<Card> c = new List<Card>
            {
                card
            };

            parentAction.PushAction(new A_CardSelection("¿<b>Poner</b> esta carta en el tope de tu mazo?", c, card.owner.photonId, this, card.instanceId).ModifyParameters(true, false, 0, 0));
        }
        else
        {
            GM.actionManager.PushAction(parentAction.actionId, new A_CardSelectionWait(GM.localPlayer.photonId, this, card.instanceId));
        }
    }


    public override void DoneSelecting(int[] cardIds)
    {
        if(cardIds != null)
        {
            parentAction.MakeActiveOnComplete(false);
            parentAction.PushAction(new A_SendToDeck(card.instanceId, card.owner.photonId, 0));
        }

        Finish();
    }
}
