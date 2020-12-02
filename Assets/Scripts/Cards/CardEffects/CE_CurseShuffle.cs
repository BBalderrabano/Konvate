using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Curse Shuffle")]
public class CE_CurseShuffle : CardEffect
{
    public bool only_owner_can_shuffle = true;

    public override void Execute()
    {
        if (only_owner_can_shuffle)
        {
            if(card.owner.photonId == card.photonId)
            {
                SendToDeck();
            }
        }
        else
        {
            SendToDeck();
        }
    }

    void SendToDeck()
    {
        PlayerHolder opponent = GM.GetOpponentHolder(card.photonId);

        parentAction.MakeActiveOnComplete(false);

        parentAction.PushAction(new A_SendToOpponentDeck(card.instanceId, card.owner.photonId, opponent.photonId));

        if (opponent.isLocal)
        {
            parentAction.PushAction(new A_Shuffle(opponent.photonId, false));
        }
    }
}
