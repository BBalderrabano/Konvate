using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu(menuName = "Card Effects/Jinete/Embestir")]
public class CE_Embestir : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        if(opponent.deck.Count > 0)
        {
            Card top_deck_card = opponent.deck.First();

            parentAction.PushAction(new A_Discard(top_deck_card.instanceId, opponent.photonId));
        }
    }
}
