
public class A_GiveControl : KAction
{
    int newControllerPhotonId;
    int phaseIndex;
    bool endMyPhase;
    bool showMessages;

    public A_GiveControl(int phaseIndex, int photonId, bool endMyPhase = false, int newControllerPhotonId = -1, bool showMessages = true, int actionId = -1) : base(photonId, actionId)
    {
        this.endMyPhase = endMyPhase;
        this.phaseIndex = phaseIndex;
        this.newControllerPhotonId = newControllerPhotonId;
        this.showMessages = showMessages;
    }

    public override bool Continue()
    {
        return false;
    }

    public override void Execute(float t)
    {
        if (!isInit)
        {
            Phase phase = GM.turn.GetPhaseByIndex(phaseIndex);

            if (endMyPhase)
            {
                phase.ChangePlayerSync(photonId, true);

                if (showMessages)
                {
                    if (photonId == GM.localPlayer.photonId)
                    {
                        WarningPanel.singleton.ShowWarning("Terminaste la fase", true);
                    }
                    else
                    {
                        WarningPanel.singleton.ShowWarning(GM.GetPlayerHolder(photonId).playerName + " terminó la fase", true);
                    }
                }
            }

            if (!phase.CheckPlayerIsReady(newControllerPhotonId))
            {
                if (showMessages)
                {
                    if (photonId == GM.localPlayer.photonId)
                    {
                        WarningPanel.singleton.ShowWarning("Cediste tu turno");
                    }
                    else
                    {
                        WarningPanel.singleton.ShowWarning(GM.GetPlayerHolder(photonId).playerName + " cedió su turno");
                    }
                }

                GM.ChangeTurnController(newControllerPhotonId);
            }

            if (photonId == GM.localPlayer.photonId)
            {
                MultiplayerManager.singleton.SendActionGiveControl(phaseIndex, photonId, endMyPhase, newControllerPhotonId, showMessages, actionId);
            }

            isInit = true;
        }
    }

    public override bool IsComplete()
    {
        return isInit;
    }
}
