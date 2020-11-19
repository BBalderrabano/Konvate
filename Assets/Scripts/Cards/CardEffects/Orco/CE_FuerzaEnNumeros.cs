using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Orco/Fuerza en Numeros")]
public class CE_FuerzaEnNumeros : CardEffect
{
    public int combat_chip_amount = 1;

    public override void Execute()
    {
        base.Execute();

        if (false)
        {
            PlayerHolder player = card.owner;

            GM.RevealCard(card);

            player.handCards.Remove(card);
            player.playedCards.Add(card);

            card.cardPhysicalInst.gameObject.SetActive(true);

            parentAction.PushAction(new Anim_FuerzaEnNumeros(player.photonId, card.instanceId, combat_chip_amount));
        }
        card.cardPhysicalInst.viz.cardBorder.color = Color.black;
        GM.HidePreviewCard();
    }
}


