using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Orco/Fuerza en Numeros")]
public class CE_FuerzaEnNumeros : CardEffect
{
    public int combat_chip_amount = 1;

    public override void Execute()
    {
        base.Execute();

        skipsEffectPreviewTime = false;

        List<Card> played_cards = GM.GetOpponentHolder(card.owner.photonId).playedCards;

        List<Card> defense = new List<Card>();

        defense.AddRange(played_cards.Where(a => a.HasTags(CardTag.DEFENSA)).ToList());
        defense.AddRange(played_cards.Where(a => a.HasTags(CardTag.DEFENSA_SUPERIOR)).ToList());

        if(defense.Count == 0)
        {
            PlayerHolder player = card.owner;

            card.RevealCard();

            player.handCards.Remove(card);
            player.playedCards.Add(card);

            card.cardPhysicalInst.gameObject.SetActive(true);

            parentAction.PushAction(new Anim_FuerzaEnNumeros(player.photonId, card.instanceId, combat_chip_amount));
            AudioManager.singleton.Play(SoundEffectType.CARD_ORCO_FUERZA_EN_NUMEROS);
        }
        else
        {
            skipsEffectPreviewTime = true;
            card.MakeBorderInactive();
            GM.HidePreviewCard();
        }
    }
}


