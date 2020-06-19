using System.Linq;
using UnityEngine;

public class A_CardSelectionComplete : Action
{
    int[] selectedCards;
    bool completedAction = false;

    public A_CardSelectionComplete(int[] card_ids, int photonId, int cardId, int effectId, int actionId = -1) : base(photonId, actionId)
    {
        this.cardOrigin = cardId;
        this.effectOrigin = effectId;
        this.selectedCards = card_ids;
    }

    public override bool Continue()
    {
        return false;
    }

    public override void Execute(float t) 
    {
        if (!completedAction)
        {
            foreach (A_CardSelectionWait selection in GM.actionManager.GetPlayerActions(GM.localPlayer.photonId).OfType<A_CardSelectionWait>())
            {
                if (selection.CompletionCheck(selectedCards, cardOrigin, effectOrigin))
                {
                    completedAction = true;
                }
            }
        }
    }

    public override bool IsComplete()
    {
        return completedAction;
    }

    public override void OnComplete()
    {
        MultiplayerManager.singleton.SendCompleteAction(actionId, GM.localPlayer.photonId);

        base.OnComplete();
    }
}
