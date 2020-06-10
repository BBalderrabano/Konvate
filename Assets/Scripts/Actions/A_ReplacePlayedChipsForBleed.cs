
public class A_ReplacePlayedChipsForBleed : Action
{
    public A_ReplacePlayedChipsForBleed(int photonId, int actionId = -1) : base(photonId, actionId) {}

    public override bool Continue()
    {
        return false;
    }

    public override void Execute(float t)
    {
        if (!isInit)
        {
            LinkAnimation(GM.animationManager.ReplaceChipsWithBleed(actionId));
            isInit = true;
        }
    }

    public override bool IsComplete()
    {
        return isInit && AnimationsAreReady();
    }
}
