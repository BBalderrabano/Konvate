
public abstract class StatModification
{
    public StatType stat_mod;

    public int amount;

    readonly bool isTemporary;

    public bool IsTemporary()
    {
        protection--;

        return protection < 0 && isTemporary;
    }

    public int identifier;

    public abstract int modify(int value);

    public StatModification(int amount = 0, bool isTemporary = true)
    {
        this.amount = amount;
        this.isTemporary = isTemporary;
        this.protection = 0;
    }

    public int protection = 0;

    public StatModification GiveProtection(int amount)
    {
        protection += amount;
        return this;
    }

    public StatModification(int amount = 0, bool isTemporary = true, int identifier = -1)
    {
        this.amount = amount;
        this.isTemporary = isTemporary;
        this.identifier = identifier;
        this.protection = 0;
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