using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Turns/Phases/Draw Phase")]
public class DrawPhase : Phase
{
    public GameState playerControlState;

    public override bool IsComplete()
    {
        if (GM.isMultiplayer)
        {
            return isInit && GM.actionManager.IsDone();
        }
        else
        {
            return true;
        }
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
            GM.SetState(null);
            GM.onPhaseChange.Raise();
            isInit = true;

            GM.DrawCard(GM.localPlayer, GM.localPlayer.ModifiedStartDrawAmount());
        }
    }
    public override void OnEndPhase()
    {
        if (isInit)
        {
            isInit = false;

            GM.currentPlayer = GM.turn.offensivePlayer;

            GM.SetState(playerControlState);

            GM.onPhaseControllerChange.Raise();
            GM.onPhaseChange.Raise();

            WarningPanel.singleton.ShowWarning(GM.currentPlayer.photonId == GM.localPlayer.photonId ? "¡Tienes la ofensiva!" : "¡Tu oponente tiene la ofensiva!", true);
        }
    }

    public override void OnPhaseControllerChange(int photonId)
    {
    }

    public override void OnTurnButtonHold(Button button)
    {
    }

    public override void OnTurnButtonPress(Button button)
    {
    }

    public override void OnPlayCard(Card c)
    {
    }
}
