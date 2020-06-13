using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Asesino/Agilidad")]
public class CE_Agilidad : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        if (card.owner.isLocal)
        {
            parentAction.PushAction(new A_Shuffle(card.owner.photonId, false));
        }
    }
}
