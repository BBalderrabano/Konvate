using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WarningPanel : MonoBehaviour, IPointerClickHandler
{
    public TMPro.TextMeshProUGUI description;
    public static WarningPanel singleton;

    Vector3 originalScale;
    bool lastMessageWasImportant = false;

    string lastEternalMessage = null;

    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }

        originalScale = this.transform.localScale;
        this.gameObject.SetActive(false);
    }
    
    public void ShowWarning(string text, bool isImportant = false, bool isEternal = false)
    {
        if (!lastMessageWasImportant || isImportant) {

            LeanTween.cancel(this.gameObject);

            this.gameObject.SetActive(true);
            description.text = text;

            this.transform.localScale = Vector3.zero;

            if (isEternal)
            {
                lastEternalMessage = text;

                LeanTween.scale(this.gameObject, originalScale, 0.5f).setEaseOutElastic();
            }
            else
            {
                LeanTween.scale(this.gameObject, originalScale, 0.5f).setEaseOutElastic().setOnComplete(Dissapear);
            }

            lastMessageWasImportant = isImportant;
        }
    }

    public void Dissapear()
    {
        if(lastEternalMessage != null)
        {
            ShowWarning(lastEternalMessage, false, true);
        }
        else
        {
            LeanTween.scale(this.gameObject, Vector3.zero, 0.5f).setDelay(1f).setEaseInElastic().setOnComplete(Disable);
        }
    }

    public void Disable()
    {
        lastMessageWasImportant = false;
        this.gameObject.SetActive(false);
        lastEternalMessage = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(lastEternalMessage == null)
        {
            LeanTween.scale(this.gameObject, Vector3.zero, 0.2f).setEaseOutSine().setOnComplete(Disable);
        }
    }
}
