using System.Collections.Generic;

public class A_CardSelection : Action
{
    List<Card> card_pool;
    string description;
    SelectionCardEffect callback;

    bool doneSelecting = false;

    public A_CardSelection(string description, List<Card> card_pool, int photonId, SelectionCardEffect callback = null, int cardId = -1, int actionId = -1) : base(photonId, actionId) 
    {
        this.card_pool = card_pool;
        this.description = description;
        this.callback = callback;

        if(callback != null)
        {
            this.cardOrigin = cardId;
            this.effectOrigin = callback.effectId;
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
            if (card_pool.Count <= 0)
            {
                DoneSelecting(null);
                return;
            }
            else
            {
                ScrollSelectionManager.singleton.SelectCards(card_pool, description, false, false, 0, 0, this);
            }

            isInit = true;
        }
    }

    public override bool IsComplete()
    {
        if(callback != null)
        {
            return isInit && doneSelecting && PlayersAreReady();
        }
        else
        {
            return isInit && doneSelecting;
        }
    }

    internal void DoneSelecting(int[] cardIds)
    {
        if(callback != null)
        {
            PlayerIsReady(photonId);
            callback.DoneSelecting(cardIds);
            doneSelecting = true;

            MultiplayerManager.singleton.PlayerFinishCardSelection(cardIds, cardOrigin, effectOrigin, photonId, actionId);
        }
    }
}
