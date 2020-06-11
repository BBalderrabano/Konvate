using UnityEngine;

public class A_ResetChips : Action
{
    public A_ResetChips(int photonId, int actionId = -1) : base(photonId, actionId) {}

    public override bool Continue()
    {
        return false;
    }

    public override void Execute(float t)
    {
        if (!isInit)
        {
            PlayerHolder p = GM.getPlayerHolder(photonId);

            foreach(Chip c in p.all_bleed_chips)
            {
                ResetPosition(c.gameObject.transform);
            }

            foreach (Chip c in p.all_combat_chips)
            {
                ResetPosition(c.gameObject.transform);
            }

            foreach (Chip c in p.all_poison_chips)
            {
                ResetPosition(c.gameObject.transform);
            }

            ResetPosition(GM.turn.offensiveChip.transform);

            isInit = true;
        }
    }

    void ResetPosition(Transform t)
    {
        t.transform.localPosition = Vector3.zero;
        t.transform.localEulerAngles = Vector3.zero;
        t.transform.localScale = Vector3.one;
    }

    public override bool IsComplete()
    {
        return isInit;
    }
}
