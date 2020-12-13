
public class A_SyncronizeCards : KAction
{
    bool hand;
    bool deck;
    bool discard;
    bool played;

    public A_SyncronizeCards(int photonId, bool hand, bool deck, bool discard, bool played, int actionId = -1) : base(photonId, actionId) 
    {
        this.hand = hand;
        this.deck = deck;
        this.discard = discard;
        this.played = played;
    }

    public override bool Continue()
    {
        return false;
    }

    public override void Execute(float t)
    {
        if (!isInit)
        {
            PlayerHolder player = GM.GetPlayerHolder(photonId);

            int[] handCards = null;
            int[] deckCards = null;
            int[] discardCards = null;
            int[] playedCards = null;

            if (hand)
            {
                handCards = new int[player.handCards.Count];

                for (int i = 0; i < handCards.Length; i++)
                {
                    handCards[i] = player.handCards[i].instanceId;
                }
            }

            if (deck)
            {
                deckCards = new int[player.deck.Count];
                for (int i = 0; i < deckCards.Length; i++)
                {
                    deckCards[i] = player.deck[i].instanceId;
                }
            }

            if (discard)
            {
                discardCards = new int[player.discardCards.Count];
                
                for (int i = 0; i < discardCards.Length; i++)
                {
                    discardCards[i] = player.discardCards[i].instanceId;
                }
            }

            if (played)
            {
                playedCards = new int[player.playedCards.Count];

                for (int i = 0; i < playedCards.Length; i++)
                {
                    playedCards[i] = player.playedCards[i].instanceId;
                }
            }


            MultiplayerManager.singleton.SyncronizeAllCards(GM.localPlayer.photonId, handCards, deckCards, discardCards, playedCards, actionId);

            PlayerIsReady(photonId);

            isInit = true;
        }
    }

    public override bool IsComplete()
    {
        return isInit && PlayersAreReady();
    }
}
