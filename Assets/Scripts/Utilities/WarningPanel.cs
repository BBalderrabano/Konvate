using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningPanel : MonoBehaviour
{
    public TMPro.TextMeshProUGUI description;
    public static WarningPanel singleton;

    Vector3 originalScale;
    bool important = false;

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
        if (!important || isImportant) {

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

            important = isImportant;
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
            LeanTween.scale(this.gameObject, Vector3.zero, 0.5f).setDelay(1f).setEaseInElastic().setOnComplete(Dissapear);
        }
    }

    public void Disable()
    {
        important = false;
        this.gameObject.SetActive(false);
        lastEternalMessage = null;
    }
}
