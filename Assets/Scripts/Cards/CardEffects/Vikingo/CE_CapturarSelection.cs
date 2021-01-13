using ExitGames.Client.Photon.StructWrapping;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Vikingo/Capturar Selection")]
public class CE_CapturarSelection : SelectionCardEffect
{
    public CE_CapturarTransfer transferEffect;
    public CE_CapturarReturn returnEffect;

    public override void Execute()
    {
        base.Execute();

        isAnimatingLeanTween = false;

        transferEffect.new_owner = card.owner;
        returnEffect.captureCard = card;

        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        List<Card> available = new List<Card>();

        available.AddRange(opponent.playedCards);

        available.RemoveAll(a => a.cardEffects.Exists(b => b.IsType<CE_CapturarReturn>()));
        
        if(available.Count == 0)
        {
            Finish();
        }
        else
        {
            if (card.owner.isLocal)
            {
                parentAction.PushAction(new A_CardSelection("<b>Captura</b> una carta", available, card.owner.photonId, this, card.instanceId));
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
            isAnimatingLeanTween = true;

            Card c = GM.GetCard(cardIds.First());
            CardEffect transfer = transferEffect.Clone(c, false);
            CardEffect returnEf = returnEffect.Clone(c, false);

            PlayerHolder opponent = c.owner;
            PlayerHolder owner = card.owner;

            c.cardViz.cardFrontImage.color = new Color(1f, 1f, 0.392f);

            c.MakeBorderInactive();
            c.cardEffects.Add(transfer);
            c.cardEffects.Add(returnEf);

            opponent.playedCards.Remove(c);

            LeanTween.move(c.cardPhysicalInst.gameObject, card.owner.currentHolder.deckGrid.value.position, Settings.ANIMATION_TIME)
            .setEaseInOutQuad()
            .setDelay(Settings.ANIMATION_DELAY)
            .setOnComplete(()=> {
                foreach (CardEffect eff in c.cardEffects)
                {
                    if (eff.type == EffectType.STARTTURN)
                    {
                        GM.turn.startTurnEffects.Add(eff);
                    }
                }

                c.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.deckLogic);
            });

            Vector3 original_scale = card.cardPhysicalInst.transform.localScale;

            LeanTween.scale(card.cardPhysicalInst.gameObject, Vector3.zero, Settings.ANIMATION_TIME).setOnComplete(() =>
            {
                owner.deck.Remove(card);
                owner.discardCards.Remove(card);
                owner.handCards.Remove(card);
                owner.playedCards.Remove(card);
                owner.all_cards.Remove(card);

                card.cardPhysicalInst.transform.localScale = original_scale;
                card.cardPhysicalInst.gameObject.SetActive(false);

                card.cardPhysicalInst.transform.SetParent(card.owner.currentHolder.deckGrid.value);
                card.cardPhysicalInst.transform.position = card.owner.currentHolder.deckGrid.value.position;

                isAnimatingLeanTween = false;
                Finish();
            }).setDelay(Settings.ANIMATION_DELAY + 0.1f);
        }
    }
}
