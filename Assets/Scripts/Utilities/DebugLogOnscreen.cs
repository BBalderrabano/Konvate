using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugLogOnscreen : MonoBehaviour
{
    string myLog;
    Queue myLogQueue = new Queue();

    public Text text;

    void Start()
    {
        text.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        myLog = logString;
        string newString = "\n [" + type + "] : " + myLog;
        myLogQueue.Enqueue(newString);
        if (type == LogType.Exception)
        {
            newString = "\n" + stackTrace;
            myLogQueue.Enqueue(newString);
        }
        myLog = string.Empty;
        foreach (string mylog in myLogQueue)
        {
            myLog += mylog;
        }
    }

    public void ShowText()
    {
        if (text.gameObject.activeSelf)
        {
            text.gameObject.SetActive(false);
        }
        else
        {
            text.gameObject.SetActive(true);
        }
        text.text = myLog;
    }
}