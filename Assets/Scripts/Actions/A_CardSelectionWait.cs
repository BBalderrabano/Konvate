public class A_CardSelectionWait : Action
{
    PlayerHolder lastCurrentPlayer;
    SelectionCardEffect callback;
    bool doneWaiting = false;
    int waitingForPlayer;

    PlayerHolder waitingForPlayerHolder;

    public A_CardSelectionWait(int photonId, SelectionCardEffect callback, int cardId, int waiting_for_photon = -1, int actionId = -1) : base(photonId, actionId)
    {
        this.callback = callback;
        this.cardOrigin = cardId;
        this.waitingForPlayer = waiting_for_photon;

        if (waitingForPlayer > 0)
        {
            waitingForPlayerHolder = GM.GetPlayerHolder(waitingForPlayer);
        }
        else
        {
            waitingForPlayerHolder = callback.card.owner;
        }
    }

    public override bool Continue()
    {
        return false;
    }

    public override void Execute(float t)
    {
        if (!isInit)
        {
            WarningPanel.singleton.ShowWarning(waitingForPlayerHolder.playerName + " esta eligiendo", true);

            lastCurrentPlayer = GM.currentPlayer;

            GM.ChangeTurnController(waitingForPlayerHolder.photonId, true);

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