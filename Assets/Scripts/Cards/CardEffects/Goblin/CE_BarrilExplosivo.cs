using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Goblin/Barril Explosivo")]
public class CE_BarrilExplosivo : CardEffect
{
    public int amount = 6;

    public override void Execute()
    {
        base.Execute();

        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        parentAction.MakeActiveOnComplete(true);

        if (opponent.playedCards.Exists(a => a.GetCardType() is NormalPlay))
        {
            AudioManager.singleton.Play(SoundEffectType.CARD_GOBLIN_BARRIL);

            iTween.ShakeScale(card.cardPhysicalInst.gameObject, new Vector3(0.2f, 0.2f), 1f);
            iTween.ShakeRotation(card.cardPhysicalInst.gameObject, new Vector3(0.2f, 0.2f), 1f);

            parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.COMBAT, amount));
            parentAction.LinkAnimation(GM.animationManager.RemoveChip(parentAction.actionId, this, card.owner.photonId, card.instanceId, ChipType.COMBAT_OFFENSIVE, amount));
        }
        else
        {
            parentAction.MakeActiveOnComplete(false);

            List<Card> send_to_deck = new List<Card>();
            PlayerHolder player = card.owner;

            send_to_deck.AddRange(player.handCards);
            send_to_deck.AddRange(player.discardCards);
            send_to_deck.AddRange(player.playedCards);

            foreach (Card c in send_to_deck)
            {
                parentAction.PushAction(new A_SendToDeck(c.instanceId, player.photonId));
            }

            if (player.isLocal)
            {
                parentAction.PushAction(new A_Shuffle(player.photonId, false));
            }
        }
    }
}
