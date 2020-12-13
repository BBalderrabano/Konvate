
public class A_MoveBleedToDamage : KAction
{
    public A_MoveBleedToDamage(int photonId, int actionId = -1) : base(photonId, actionId) {}

    public override bool Continue()
    {
        return false;
    }

    public override void Execute(float t)
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
