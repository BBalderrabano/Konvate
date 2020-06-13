
public class A_CardSelectionWait : Action
{
    PlayerHolder lastCurrentPlayer;
    SelectionCardEffect callback;
    bool doneWaiting = false;

    public A_CardSelectionWait(int photonId, SelectionCardEffect callback, int cardId, int actionId = -1) : base(photonId, actionId)
    {
        this.callback = callback;
        this.cardOrigin = cardId;
    }

    public override bool Continue()
    {
        return false;
    }

    public override void Execute(float t) 
    {
        if (!isInit)
        {
            lastCurrentPlayer = GM.currentPlayer;
            GM.ChangeTurnController(callback.card.owner.photonId, true);

            isInit = true;
        }
    }

    public bool CompletionCheck(int[] selected_cards, int cardCheck, int effectCheck)
    {
        if (!doneWaiting)
        {
            if (cardCheck == cardOrigin && effectCheck == callback.effectId)
            {
                callback.DoneSelecting(selected_cards);

                GM.ChangeTurnController(lastCurrentPlayer == null ? -1 : lastCurrentPlayer.photonId, true);

                doneWaiting = true;
                return true;
            }
        }

        return false;
    }

    public override bool IsComplete()
    {
        return doneWaiting;
    }
}
