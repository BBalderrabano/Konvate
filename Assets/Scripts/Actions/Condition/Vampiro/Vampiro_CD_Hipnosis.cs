using UnityEngine;

[CreateAssetMenu(menuName = "Conditions/Vampiro/Hipnosis")]
public class Vampiro_CD_Hipnosis : Condition
{
    [System.NonSerialized]
    public Card mustPlayCard;

    public override bool isTemporary()
    {
        return true;
    }

    public override bool isValid(int photonId, int cardId)
    {
        PlayerHolder player = GameManager.singleton.GetPlayerHolder(photonId);

        if (GameManager.singleton.GetCard(cardId).GetCardType() == mustPlayCard.GetCardType() && !player.playedCards.Contains(mustPlayCard))
        {
            WarningPanel.singleton.ShowWarning("Primero debes jugar tu " + mustPlayCard.cardName);
            return false;
        }

        return true;
    }
}
