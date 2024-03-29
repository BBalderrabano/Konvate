﻿
public class A_Shuffle : KAction
{
    int[] deck;
    bool andDraw;
    float time = 0;

    public A_Shuffle(int photonId, bool andDraw = true, int actionId = -1 ) : base(photonId, actionId)
    {
        this.andDraw = andDraw;
    }

    public A_Shuffle(int photonId, int[] deck, bool andDraw = true, int actionId = -1) : base(photonId, actionId)
    {
        this.andDraw = andDraw;
        this.deck = deck;
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

                if (andDraw)
                {
                    KAction draw = new A_Draw(photonId);
                    PushAction(draw);
                }
            }
            else
            {
                player.deck.Clear();
                player.discardCards.Clear();

                for (int i = 0; i < deck.Length; i++)
                {
                    Card c = GM.GetCard(deck[i]);

                    player.deck.Add(c);
                    Settings.SetParent(c.cardPhysicalInst.transform, player.currentHolder.deckGrid.value);
                    c.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.deckLogic);
                }

                PlayerIsReady(photonId);    //Modificar esto para que se envie LUEGO de la animacion
            }

            AudioManager.singleton.Play(SoundEffectType.SHUFFLE_DECK);

            isInit = true;
        }

        if (isInit && PlayersAreReady() && time > Settings.SHUFFLE_MIN_PREVIEW)
        {
            ExecuteLinkedAction(t);
        }

        time += t;
    }

    public override bool IsComplete()
    {
        if (GM.isMultiplayer)
        {
            return isInit && PlayersAreReady() && LinkedActionsReady();
        }
        else
        {
            return isInit && LinkedActionsReady();
        }
    }
}
