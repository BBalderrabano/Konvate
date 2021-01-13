using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Berzerker/Doble Golpe")]
public class CE_DobleGolpe : SelectionCardEffect
{
    [SerializeField]
    TextMod_DobleGolpe textMod;

    public override void Execute()
    {
        base.Execute();

        textMod.Init(card);

        skipsEffectPreviewTime = false;
        parentAction.MakeActiveOnComplete(true);

        List<Card> available_cards = card.owner.playedCards.FindAll(a => a.GetCardType() is NormalPlay && a.photonId == card.owner.photonId && a.cardName != card.cardName);

        if (available_cards.Count > 0)
        {
            if (card.owner.isLocal)
            {
                parentAction.PushAction(new A_CardSelection("<b>Copia</b> una carta en juego", available_cards, card.owner.photonId, this, card.instanceId));
            }
            else
            {
                GM.actionManager.PushAction(parentAction.actionId, new A_CardSelectionWait(GM.localPlayer.photonId, this, card.instanceId));
            }
        }
        else
        {
            skipsEffectPreviewTime = true;
        }
    }

    public override void DoneSelecting(int[] cardIds)
    {
        if (cardIds != null)
        {
            Card c = GM.GetCard(cardIds.First());

            textMod.copied_card = c;
            textMod.UpdateText();

            for (int i = 0; i < c.cardEffects.Count; i++)
            {
                card.cardEffects.Add(c.cardEffects[i].Clone(card));
            }

            GM.turn.currentPhase.value.OnPlayCard(card);
        }
    }
}
