
public class A_ExecuteEffect : Action
{
    Card owner;
    CardEffect effect;

    float time = 0;

    public A_ExecuteEffect(int cardId, int effectId, int photonId, int actionId = -1) : base(GameManager.singleton.localPlayer.photonId, actionId)
    {
        owner = GM.getPlayerHolder(photonId).GetCard(cardId);
        effect = owner.GetEffect(effectId);
        effect.parentAction = this;

        cardOrigin = cardId;
        effectOrigin = effectId;
    }

    public override bool Continue()
    {
        return false;
    }

    public override void Execute(float t)
    {
        if (!isInit)
        {
            time = 0;
            effect.Execute();
            isInit = true;
        }

        ExecuteLinkedAction(t);

        time += t;
    }

    public override bool IsComplete()
    {
        return isInit && LinkedActionsReady() && AnimationsAreReady() && (time > 1);
    }

    public override void OnComplete()
    {
        readyToRemove = true;

        effect.Finish();

        if (owner.EffectsDone())
        {
            GM.ActiveViz(owner);
        }
    }
}
