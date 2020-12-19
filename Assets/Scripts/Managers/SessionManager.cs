using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour
{
    public static SessionManager singleton;
    public delegate void OnSceneLoaded();
    public OnSceneLoaded onSceneLoaded;
    public List<GameObject> dontDestroyOnLoadObjects = new List<GameObject>();

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void LoadGameLevel()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

    public void LoadMenu()
    {
        for (int i = 0; i < dontDestroyOnLoadObjects.Count; i++)
        {
            Destroy(dontDestroyOnLoadObjects[i]);
        }

        StartCoroutine(DisconnectAndLoadLevel("Menu"));
    }

    IEnumerator DisconnectAndLoadLevel(string level)
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LeaveRoom();

        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }

        yield return SceneManager.LoadSceneAsync(level, LoadSceneMode.Single);

        if (onSceneLoaded != null)
        {
            onSceneLoaded();
            onSceneLoaded = null;
        }
    }
}
