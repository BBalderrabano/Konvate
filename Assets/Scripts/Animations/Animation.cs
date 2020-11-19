
public class Animation
{
    public int animId;
    int total;
    int amount;

    public Animation(int total = 1)
    {
        this.total = total;
        this.amount = 0;
        this.animId = GameManager.singleton.resourcesManager.GetAnimationIndex();
    }

    public virtual void OnComplete()
    {
        amount++;
    }

    public virtual bool isDone()
    {
        return amount >= total;
    }
}
