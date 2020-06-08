using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPrint : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public string playerName;
    public int photonId;
    public bool isLocal;

    public PlayerHolder playerHolder;

    string[] cardIds;

    public string[] getStartingCardIds()
    {
        return cardIds;
    }

    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        photonId = photonView.InstantiationId;
        isLocal = photonView.IsMine;

        playerName = (string)photonView.Owner.CustomProperties["player_name"];
        cardIds = (string[])photonView.Owner.CustomProperties["deck"];

        MultiplayerManager.singleton.AddPlayer(this);
    }
}
