using UnityEngine;

[CreateAssetMenu(menuName = "Turns/Phases/Discard Phase")]
public class DiscardPhase : Phase
{ 
    public override bool IsComplete()
    {
        return isInit && GM.actionManager.IsDone();
    }
    public override bool CanPlayCard(Card c)
    {
        return false;
    }

    public override void OnStartPhase()
    {
        base.OnStartPhase();

        if (!isInit)
        {
            foreach (PlayerHolder player in GM.allPlayers)
            {
                foreach (Card c in player.playedCards)
                {
                    if (c.isBroken)
                        continue;

                    foreach (CardEffect eff in c.cardEffects)
                    {
                        if (eff.type == EffectType.ENDTURN || eff.type == EffectType.ENDTURNSTART)
                        {
                            GM.turn.endTurnEffects.Add(eff);
                        }

                        if (eff.type == EffectType.STARTTURN)
                        {
                            GM.turn.startTurnEffects.Add(eff);
                        }
                    }
                }
            }

            foreach (PlayerHolder player in GM.allPlayers)
            {
                KAction resetChips = new A_ResetChips(player.photonId);

                GM.actionManager.AddAction(resetChips);
            }

            foreach (PlayerHolder player in GM.allPlayers)
            {
                KAction discardAllCards = new A_DiscardAllCards(player.photonId);

                GM.actionManager.AddAction(discardAllCards);
            }

            isInit = true;
        }
    }
    public override void OnEndPhase()
    {
        if (isInit)
        {
            GM.SetState(null);
            isInit = false;
        }
    }

    public override void OnPhaseControllerChange(int photonId)
    {
    }

    public override void OnTurnButtonHold()
    {
    }

    public override void OnTurnButtonPress()
    {
    }

    public override void OnPlayCard(Card c)
    {
    }
}
