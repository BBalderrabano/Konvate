using Photon.Realtime;
using SO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : Photon.Pun.MonoBehaviourPunCallbacks
{
    public static bool isMaster;
    public static NetworkManager singleton;

    ResourcesManager rm;

    public StringVariable logger;
    public GameEvent loggerUpdate;
    public GameEvent onConnected;
    public GameEvent onFailToConnect;
    public GameEvent onWaitingForPlayer;

    bool retryConnection = false;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        foreach (RoomInfo ri in roomList)
        {
            Debug.Log(ri.PlayerCount);
            Debug.Log(ri.masterClientId);
        }
    }

    public void Awake()
    {
        if(singleton == null)
        {
            rm = Resources.Load("ResourcesManager") as ResourcesManager;
            singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        propertiesInit = false;
        retryConnection = false;
        isMaster = false;

        Photon.Pun.PhotonNetwork.AutomaticallySyncScene = true;
        Init();
    }

    public void Init()
    {
        rm.Init();
        Photon.Pun.PhotonNetwork.ConnectUsingSettings();
        logger.value = "Conectando...";
        loggerUpdate.Raise();
    }

    #region My Calls

    bool propertiesInit = false;

    public void OnPlayGame()
    {
        if (!propertiesInit)
        {
            PlayerProfile profile = Resources.Load("PlayerProfile") as PlayerProfile;

            Hashtable hash = new Hashtable
            {
                { "deck", profile.cardIds },
                { "player_name", profile.playerName },
                { "PlayerIsReady", false }
            };

            Photon.Pun.PhotonNetwork.LocalPlayer.NickName = profile.playerName;
            Photon.Pun.PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

            propertiesInit = true;
        }

        JoinRandomRoom();
    }

    void JoinRandomRoom()
    {
        Photon.Pun.PhotonNetwork.JoinRandomRoom();
    }

    public void RetryJoinRandomRoom()
    {
        retryConnection = true;
        Photon.Pun.PhotonNetwork.LeaveRoom();
        isMaster = false;
    }

    public void CreateRoom()
    {
        RoomOptions room = new RoomOptions();
        room.MaxPlayers = 2;
        room.EmptyRoomTtl = 0;
        Photon.Pun.PhotonNetwork.CreateRoom(RandomString(256), room, TypedLobby.Default);
    }

    public void EndConnection()
    {
        if (Photon.Pun.PhotonNetwork.InRoom)
        {
            Photon.Pun.PhotonNetwork.LeaveRoom();
        }

        isMaster = false;
        retryConnection = false;

        logger.value = "Conectado";
        loggerUpdate.Raise();
    }

    private System.Random random = new System.Random();

    public string RandomString(int lenght)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefgolkipjklmnopqrstuvhui";
        return new string(Enumerable.Repeat(chars, lenght).Select(s => s[random.Next(s.Length)]).ToArray());
    }
    #endregion

    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        if (retryConnection)
        {
            JoinRandomRoom();
            retryConnection = false;
        }
        else
        {
            logger.value = "Conectado";
            loggerUpdate.Raise();
            onConnected.Raise();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        CreateRoom();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        logger.value = "Falló la conexión";
        loggerUpdate.Raise();
        onFailToConnect.Raise();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (isMaster)
        {
            if(Photon.Pun.PhotonNetwork.PlayerList.Length > 1)
            {
                logger.value = "Listo para jugar";
                loggerUpdate.Raise();

                Photon.Pun.PhotonNetwork.CurrentRoom.IsOpen = false;
                Photon.Pun.PhotonNetwork.Instantiate("MultiplayerManager", Vector3.zero, Quaternion.identity, 0);
            }
        }
    }

    public void LoadGameScene()
    {
        SessionManager.singleton.LoadGameLevel();
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        isMaster = true;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        logger.value = "Esperando a un jugador";
        loggerUpdate.Raise();
        onWaitingForPlayer.Raise();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        EndGameScreen.singleton.EndGame(true, "Oponente se a desconectado");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
    }

    #endregion
}
