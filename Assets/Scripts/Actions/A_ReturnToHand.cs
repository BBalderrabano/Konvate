﻿
public class A_ReturnToHand : KAction
{
    PlayerHolder player;
    Card card;

    public A_ReturnToHand(int instanceId, int photonId, int actionId = -1) : base(photonId, actionId)
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
            player.handCards.Add(card);
            player.playedCards.Remove(card);

            if (player.isLocal)
            {
                card.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.handLogic);
            }
            else
            {
                card.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.opponentHandLogic);
            }

            player.ChangeMana(+card.ModifiedCardCost());

            card.cardPhysicalInst.gameObject.SetActive(true);

            LinkAnimation(GM.animationManager.MoveCard(actionId, photonId, card.instanceId, player.currentHolder.handGrid.value.position, player.currentHolder.handGrid.value.gameObject));
           
            AudioManager.singleton.Play(SoundEffectType.PICK_CARD_UP);

            if (player.isLocal)
            {
                MultiplayerManager.singleton.SendUseCard(card.instanceId, photonId, actionId, ActionType.RETURN_TO_HAND);
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
}
