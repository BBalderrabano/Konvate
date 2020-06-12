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

            for (int i = 0; i < p.currentHolder.bleedChipHolder.value.childCount; i++)
            {
                ResetPosition(p.currentHolder.bleedChipHolder.value.GetChild(i).transform);
            }

            for (int i = 0; i < p.currentHolder.combatChipHolder.value.childCount; i++)
            {
                ResetPosition(p.currentHolder.combatChipHolder.value.GetChild(i).transform);
            }

            for (int i = 0; i < p.currentHolder.poisonChipHolder.value.childCount; i++)
            {
                ResetPosition(p.currentHolder.poisonChipHolder.value.GetChild(i).transform);
            }

            ResetPosition(GM.turn.offensiveChip.transform);

            isInit = true;
        }
    }

    void ResetPosition(Transform t)
    {
        t.localPosition = Vector3.zero;
        t.localEulerAngles = Vector3.zero;
        t.localScale = Vector3.one;
    }

    public override bool IsComplete()
    {
        return isInit;
    }
}
