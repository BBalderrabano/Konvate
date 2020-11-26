using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu(menuName = "Card Effects/Jinete/Arremeter")]
public class CE_Arremeter : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        PlayerHolder player = card.owner;
        PlayerHolder opponent = GM.GetOpponentHolder(player.photonId);

        if (opponent.deck.Count > 0)
        {
            Card top_deck_card = opponent.deck.First();
            
            parentAction.PushAction(new A_Discard(top_deck_card.instanceId, opponent.photonId));

            if (!top_deck_card.HasTags(CardTag.ATAQUE_BASICO) && !top_deck_card.HasTags(CardTag.DEFENSA))
            {
                parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, player.photonId, card.instanceId, ChipType.COMBAT, 1));
                
                if(opponent.deck.Count > 1)
                {
                    Card second_top_deck_card = opponent.deck[1];
                    parentAction.PushAction(new A_Discard(second_top_deck_card.instanceId, opponent.photonId));
                }
            }
        }
    }
}
