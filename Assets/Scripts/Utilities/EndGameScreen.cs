using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    public GameObject victoryContainer;
    public GameObject defeatContainer;

    public TMPro.TextMeshProUGUI description;

    public bool isWinner = false;

    [System.NonSerialized]
    public static EndGameScreen singleton;

    public void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        this.gameObject.SetActive(false);
    }

    public void EndGame(bool isWinner, string description)
    {
        if(!this.gameObject.activeInHierarchy)
        {
            this.description.text = description;
            this.isWinner = isWinner;

            this.gameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {
        if (isWinner)
        {
            defeatContainer.transform.parent.gameObject.SetActive(false);
            victoryContainer.transform.parent.gameObject.SetActive(true);

            victoryContainer.transform.localScale = Vector3.one;

            iTween.ScaleTo(victoryContainer, iTween.Hash(
                        "scale", new Vector3(2, 2, 2),
                        "time", 1f,
                        "easetype", "easeOutElastic"
                        ));
        }
        else
        {
            victoryContainer.transform.parent.gameObject.SetActive(false);
            defeatContainer.transform.parent.gameObject.SetActive(true);

            defeatContainer.transform.localScale = Vector3.one;

            iTween.ScaleTo(defeatContainer, iTween.Hash(
                        "scale", new Vector3(2, 2, 2),
                        "time", 1f,
                        "easetype", "easeOutQuad"
                        ));
        }
        
    }
}