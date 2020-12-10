using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Turns/Phases/Set Cards Phase")]

public class SetCardsPhase : Phase
{
    public GameState playerControlState;

    public override bool IsComplete()
    {
        return isInit && GM.actionManager.IsDone() && PlayersAreReady();
    }

    public override void OnStartPhase()
    {
        base.OnStartPhase();

        if (!isInit)
        {
            GM.SetState(playerControlState);

            GM.onPhaseChange.Raise();
            GM.currentPlayer = GM.localPlayer;
            GM.onPhaseControllerChange.Raise();

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

    public override bool CanPlayCard(Card c)
    {
        if (c.GetCardType() is NormalPlay) {

            return true;
        }
        else
        {
            WarningPanel.singleton.ShowWarning("No puedes jugar cartas relámpago (<sprite=3>) durante la fase de preparación", false);
            return false;
        }
    }

    public override void OnTurnButtonPress()
    {
        int localPlayerId = GM.localPlayer.photonId;
        int otherPlayerId = GM.clientPlayer.photonId;

        if (CheckPlayerIsReady(localPlayerId) || !GM.currentPlayer.isLocal)
            return;

        Action giveControl = new A_GiveControl(phaseIndex, localPlayerId, true, otherPlayerId);
        GM.actionManager.AddAction(giveControl);

        AudioManager.singleton.Play(SoundEffectType.BUTTON_CLICK);
    }

    public override void OnTurnButtonHold()
    {
        OnTurnButtonPress();
    }

    public override void OnPhaseControllerChange(int photonId)
    {
    }

    public override void OnPlayCard(Card c)
    {
    }
}
