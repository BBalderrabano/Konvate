using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Pestilente/Maestria del Veneno")]
public class CE_MaestriaDelVeneno : CardEffect
{
    bool hasPoison = false;

    bool isInit = false;
    List<Card> cardsDrawn = new List<Card>();

    public override void Execute()
    {
        base.Execute();

        if (!isInit)
        {
            isInit = true;
            cardsDrawn.Clear();
        }

        if (GameManager.singleton.isMultiplayer && card.owner.isLocal)
        {
            ///TODO: Implement action based card draw
            List<Card> cardDrawn = new List<Card>(); //GameManager.singleton.DrawCard(card.owner, 1, card.instanceId, effectId);

            if(cardDrawn.Count > 0)
            {
                hasPoison = cardDrawn[0].HasTags(new CardTags[] { CardTags.PLACES_POISON_CHIP });
                cardsDrawn.AddRange(cardDrawn);
            }
            else
            {
                isDone = true;
            }
        }
    }

    public override void Finish()
    {
        if (card.owner.isLocal)
        {
            if (hasPoison)
            {
                Execute();
            }
            else
            {
                //MultiplayerManager.singleton.EndCardEffect(card.owner.photonId, card.instanceId, effectId, true);

                int[] cardIds = new int[cardsDrawn.Count];

                for (int i = 0; i < cardIds.Length; i++)
                {
                    cardIds[i] = cardsDrawn[i].instanceId;
                }

                MultiplayerManager.singleton.ShowOpponentCards(cardIds, card.owner.photonId, "Maestria del Veneno");

                isInit = false;
            }
        }
    }

}
