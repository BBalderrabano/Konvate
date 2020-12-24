
public abstract class StatModification
{
    public StatType stat_mod;

    public int amount;

    public bool isTemporary;

    public int identifier;

    public abstract int modify(int value);

    public StatModification(int amount = 0, bool isTemporary = true)
    {
        this.amount = amount;
        this.isTemporary = isTemporary;
    }
    
    public StatModification(int amount = 0, bool isTemporary = true, int identifier = -1)
    {
        this.amount = amount;
        this.isTemporary = isTemporary;
        this.identifier = identifier;
    }
}

public enum StatType
{
    ENERGY_COST,
    START_DRAW_AMOUNT,
    QUICK_PLAY,
    START_ENERGY_AMOUNT,
    DAMAGE_MODIFIER,
    TRYNDAMERE,
    PREVENT_FIST_TO_BLEED
}