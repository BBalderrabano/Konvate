using System.Collections.Generic;
using System.Linq;

public class A_CardSelection : KAction
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

        isMultiple = false;
        minSelected = 0;
        maxSelected = 0;

        if (callback != null)
        {
            this.cardOrigin = cardId;
            this.effectOrigin = callback.effectId;
        }
    }

    bool isYesNo = false;
    bool isMultiple = false;
    int minSelected = 0;
    int maxSelected = 0;

    public A_CardSelection ModifyParameters(bool isYesNo, bool isMultiple, int minSelected, int maxSelected)
    {
        this.isMultiple = isMultiple;
        this.minSelected = minSelected;
        this.maxSelected = maxSelected;
        this.isYesNo = isYesNo;

        return this;
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
                if (!isYesNo)
                {
                    ScrollSelectionManager.singleton.SelectCards(card_pool, description, false, isMultiple, minSelected, maxSelected, this);
                }
                else
                {
                    ScrollSelectionManager.singleton.YesNoSelection(card_pool.First(), description, this);
                }
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
