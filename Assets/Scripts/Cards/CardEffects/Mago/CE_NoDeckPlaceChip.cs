using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Mago/No deck place chip")]
public class CE_NoDeckPlaceChip : CardEffect
{
    public ChipType chip_type;
    public int amount = 1;

    public override void Execute()
    {
        base.Execute();

        PlayerHolder player = card.owner;

        if(player.discardCards.Count == 0)
        {
            iTween.PunchPosition(card.cardPhysicalInst.gameObject, new Vector3(0, 0.2f), 0.5f);
            parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, player.photonId, card.instanceId, chip_type, amount));
            AudioManager.singleton.Play(SoundEffectType.CARD_MAGO_NO_DECK);
        }
    }
}
