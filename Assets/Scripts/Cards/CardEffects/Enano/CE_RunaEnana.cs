using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Enano/Runa Enana")]
public class CE_RunaEnana : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        int quick_play_amount = 0;

        foreach(PlayerHolder player in GM.allPlayers)
        {
            quick_play_amount += player.playedCards.Where(a => a.GetCardType() is QuickPlay).ToList().Count;
        }

        if(quick_play_amount > 0)
        {
            parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.COMBAT, quick_play_amount));
        }
    }
}
