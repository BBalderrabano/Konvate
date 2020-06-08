﻿
public class A_Shuffle : Action
{
    int[] deck;
    bool andDraw;
    bool drawCreated = false;

    public A_Shuffle(int photonId, bool andDraw = true, int instanceId = -1 ) : base(photonId, instanceId){
        this.andDraw = andDraw;
    }

    public A_Shuffle(int photonId, int[] deck, bool andDraw = true, int instanceId = -1) : base(photonId, instanceId)
    {
        this.andDraw = andDraw;
        this.deck = deck;
    }

    public override bool Continue()
    {
        return false;
    }

    public override void Execute()
    {
        if (!isInit)
        {
            isInit = true;
            PlayerHolder player = GM.getPlayerHolder(photonId);

            if (player.isLocal || !GM.isMultiplayer)
            {
                foreach (Card c in player.discardCards)
                {
                    player.deck.Add(c);
                    Settings.SetParent(c.cardPhysicalInst.transform, player.currentHolder.deckGrid.value);
                    c.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.deckLogic);
                }

                player.deck.Shuffle();
                player.discardCards.Clear();

                if (GM.isMultiplayer)
                {
                    int[] deck_state = new int[player.deck.Count];

                    for (int i = 0; i < player.deck.Count; i++)
                    {
                        deck_state[i] = player.deck[i].instanceId;
                    }

                    PlayerIsReady(photonId);    //Modificar esto para que se envie LUEGO de la animacion
                    MultiplayerManager.singleton.SendShuffle(actionId, photonId, deck_state);
                }
            }
            else
            {
                player.deck.Clear();
                player.discardCards.Clear();

                for (int i = 0; i < deck.Length; i++)
                {
                    Card c = player.GetCard(deck[i]);

                    player.deck.Add(c);
                    Settings.SetParent(c.cardPhysicalInst.transform, player.currentHolder.deckGrid.value);
                    c.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.deckLogic);
                }

                PlayerIsReady(photonId);    //Modificar esto para que se envie LUEGO de la animacion

                MultiplayerManager.singleton.SendAction(actionId, player.photonId);
            }
        }
    }

    public override bool IsComplete()
    {
        if (GM.isMultiplayer)
        {
            if (andDraw && !drawCreated)
            {
                PlayerHolder player = GM.getPlayerHolder(photonId);

                if (player.isLocal)
                {
                    Action draw = new A_Draw(photonId);
                    PushAction(draw);
                }

                drawCreated = true;
            }

            return PlayersAreReady();
        }
        else
        {
            return true;
        }
    }
}
