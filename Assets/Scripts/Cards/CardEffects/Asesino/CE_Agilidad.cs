using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Asesino/Agilidad")]
public class CE_Agilidad : CardEffect
{
    public override void Execute()
    {
        PlayerHolder player = card.owner;
        Card discardCard;

        for (int i = 0; i < card.owner.discardCards.Count; i++)
        {
            discardCard = card.owner.discardCards[i];

            discardCard.cardPhysicalInst.setCurrentLogic(GameManager.singleton.resourcesManager.dataHolder.deckLogic);
            Settings.SetParent(discardCard.cardPhysicalInst.transform, player.currentHolder.deckGrid.value);
        }

        player.deck.AddRange(card.owner.discardCards);
        player.discardCards.Clear();

        player.deck.Shuffle();
        player.deck.Shuffle();
        player.deck.Shuffle();

        int[] deckOrder = new int[player.deck.Count];

        for (int e = 0; e < deckOrder.Length; e++)
        {
            deckOrder[e] = player.deck[e].instanceId;
        }

        Action sync = new A_SyncronizeCards(player.photonId, false, true, false, false);
        parentAction.PushAction(sync);

        base.Finish();
    }
}
