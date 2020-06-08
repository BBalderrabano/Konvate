
public class A_MoveBleedToDamage : Action
{
    public A_MoveBleedToDamage(int photonId, int actionId = -1) : base(photonId, actionId) {}

    public override bool Continue()
    {
        return false;
    }

    public override void Execute()
    {
        if (!isInit)
        {
            LinkAnimation(GM.animationManager.MoveBleedChipsToDamagePlayers(actionId));
            isInit = true;
        }
    }

    public override bool IsComplete()
    {
        return isInit && AnimationsAreReady();
    }
}
