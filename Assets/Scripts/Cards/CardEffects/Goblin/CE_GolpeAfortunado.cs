using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Goblin/Golpe Afortunado")]
public class CE_GolpeAfortunado : SelectionCardEffect
{
    public int amount = 1;
    PlayerHolder opponent;

    public override void Execute()
    {
        base.Execute();

        skipsEffectPreviewTime = false;

        opponent = GM.GetOpponentHolder(card.owner.photonId);

        if(opponent.deck.Count < 1)
        {
            skipsEffectPreviewTime = true;
            Finish();
        }
        else
        {
            if (card.owner.isLocal)
            {
                parentAction.PushAction(new A_CardSelection("¿Es <sprite=3> la carta del fondo del mazo oponente?", card, base.card.photonId, this, base.card.instanceId).ModifyParameters(true, false, 0, 0));
            }
            else
            {
                GM.actionManager.PushAction(parentAction.actionId, new A_CardSelectionWait(GM.localPlayer.photonId, this, card.instanceId, opponent.photonId));
            }
        }
    }

    public override void DoneSelecting(int[] cardIds)
    {
        GM.PreviewCard(opponent.deck.Last());

        if (cardIds == null)
        {
            if(opponent.deck.Last().GetCardType() is NormalPlay)
            {
                PlaceAnotherChip();
            }
        }
        else
        {
            if (opponent.deck.Last().GetCardType() is QuickPlay)
            {
                PlaceAnotherChip();
            }
        }
    }

    void PlaceAnotherChip()
    {
        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.COMBAT, amount));
    }
}
