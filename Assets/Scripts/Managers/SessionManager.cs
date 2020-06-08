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
        StartCoroutine(LoadLevel("Menu"));
    }

    IEnumerator LoadLevel(string level)
    {
        
        yield return SceneManager.LoadSceneAsync(level, LoadSceneMode.Single);
        if(onSceneLoaded != null)
        {
            onSceneLoaded();
            onSceneLoaded = null;
        }
    }
}
