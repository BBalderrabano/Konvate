﻿
public class A_PlayCard : KAction
{
    PlayerHolder player;
    Card card;

    public A_PlayCard(int instanceId, int photonId, int actionId = -1) : base(photonId, actionId)
    {
        player = GM.GetPlayerHolder(photonId);
        card = GM.GetPlayerHolder(photonId).GetCard(instanceId);
    }

    public override bool Continue()
    {
        return false;
    }

    public override void Execute(float t)
    {
        if (!isInit)
        {
            if (card.GetCardType() == GM.resourcesManager.dataHolder.setPlayType)
            {
                card.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.cardFaceDownLogic);
            }
            else if (card.GetCardType() == GM.resourcesManager.dataHolder.quickPlayType)
            {
                player.playedQuickCard = true;
                card.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.cardQuickPlayLogic);
            }
            else
            {
                card.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.cardFaceDownLogic);
            }

            foreach (CardEffect eff in card.cardEffects)
            {
                if (eff.type == EffectType.ON_PLAY_START)
                {
                    eff.Execute();
                }
            }

            player.handCards.Remove(card);
            player.playedCards.Add(card);

            player.ChangeMana(-card.ModifiedCardCost());

            card.cardPhysicalInst.gameObject.SetActive(true);

            LinkAnimation(GM.animationManager.MoveCard(actionId, photonId, card.instanceId, player.currentHolder.playedGrid.value.position, player.currentHolder.playedGrid.value.gameObject, SoundEffectType.PLACE_CARD));

            if (player.isLocal)
            {
                MultiplayerManager.singleton.SendUseCard(card.instanceId, photonId, actionId, ActionType.PLAY_CARD);
            }

            isInit = true;
        }
    }

    public override bool IsComplete()
    {
        if (GM.isMultiplayer)
        {
            return PlayersAreReady();
        }
        else
        {
            return true;
        }
    }

    bool completed = false;

    public override void OnComplete()
    {
        if (!completed)
        {
            GM.turn.currentPhase.value.OnPlayCard(card);
            completed = true;

            foreach (CardEffect eff in card.cardEffects)
            {
                if (eff.type == EffectType.ON_PLAY_END)
                {
                    eff.Execute();
                }
            }
        }

        base.OnComplete();
    }
}
