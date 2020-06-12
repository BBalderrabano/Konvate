using UnityEngine;
using System.Collections;

public class A_PickCard : Action
{
    public A_PickCard(int photonId, int actionId = -1) : base(photonId, actionId) {}

    public override bool Continue()
    {
        throw new System.NotImplementedException();
    }

    public override void Execute(float t)
    {
        throw new System.NotImplementedException();
    }

    public override bool IsComplete()
    {
        throw new System.NotImplementedException();
    }
}
