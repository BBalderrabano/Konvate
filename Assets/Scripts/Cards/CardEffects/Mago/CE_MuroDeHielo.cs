using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Mago/Muro de Hielo")]
public class CE_MuroDeHielo : CardEffect
{
    public int amount = -1;

    public override void Execute()
    {
        base.Execute();

        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        opponent.ChangeMana(amount);
    }

    public override void OnLeavePlay()
    {
        base.OnLeavePlay();

        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        opponent.ChangeMana(Mathf.Abs(amount));
    }
}
