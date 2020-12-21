using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Golem/Avalancha")]
public class CE_Avalancha : CardEffect
{
    public Card stoneSkin;

    public override void Execute()
    {
        base.Execute();

        PlayerHolder player = card.owner;

        int amount = player.playedCards.FindAll(a => a.cardName == stoneSkin.cardName || a.HasTags(CardTag.DEFENSA_SUPERIOR)).Count;

        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, player.photonId, card.instanceId, ChipType.COMBAT, Mathf.Max(1, amount)));
    }
}
