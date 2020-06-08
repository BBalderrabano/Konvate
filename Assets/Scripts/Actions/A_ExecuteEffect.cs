
public class A_ExecuteEffect : Action
{
    Card owner;
    CardEffect effect;
    
    public A_ExecuteEffect(int cardId, int effectId, int photonId, int actionId = -1) : base(GameManager.singleton.localPlayer.photonId, actionId)
    {
        owner = GM.getPlayerHolder(photonId).GetCard(cardId);
        effect = owner.GetEffect(effectId);
        effect.parentAction = this;
    }

    public override bool Continue()
    {
        if (linkedActions.Count > 0)
        {
            return true;
        }
        else
        {
            return forceContinue;
        }
    }

    public override void Execute()
    {
        if (!isInit)
        {
            effect.Execute();
            isInit = true;
        }
    }

    public override bool IsComplete()
    {
        return effect.isDone && AnimationsAreReady();
    }

    public override void OnComplete()
    {
        base.OnComplete();

        if (owner.EffectsDone())
        {
            GM.ActiveViz(owner);
        }
    }
}
