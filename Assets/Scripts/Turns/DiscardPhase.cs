using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Turns/Phases/Discard Phase")]
public class DiscardPhase : Phase
{ 
    public override bool IsComplete()
    {
        return isInit && GM.actionManager.IsDone();
    }
    public override bool CanPlayCard(Card c)
    {
        return false;
    }

    public override void OnStartPhase()
    {
        base.OnStartPhase();

        if (!isInit)
        {
            foreach (PlayerHolder player in GM.allPlayers)
            {
                Action resetChips = new A_ResetChips(player.photonId);

                GM.actionManager.AddAction(resetChips);
            }

            foreach (PlayerHolder player in GM.allPlayers)
            {
                Action discardAllCards = new A_DiscardAllCards(player.photonId);

                GM.actionManager.AddAction(discardAllCards);
            }

            isInit = true;
        }
    }
    public override void OnEndPhase()
    {
        if (isInit)
        {
            GM.SetState(null);
            isInit = false;
        }
    }

    public override void OnPhaseControllerChange(int photonId)
    {
    }

    public override void OnTurnButtonHold()
    {
    }

    public override void OnTurnButtonPress()
    {
    }

    public override void OnPlayCard(Card c)
    {
    }
}
