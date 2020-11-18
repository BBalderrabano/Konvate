using UnityEngine;
using TMPro;

public class CountChips : MonoBehaviour
{
    public Transform checkFrom;
    int previousCount = 0;

    private void Start()
    {
        previousCount = checkFrom.childCount;    //The image and the score text itself get substracted
        GetComponent<TextMeshProUGUI>().text = "x" + previousCount;
    }

    void Update()
    {
        int count = checkFrom.childCount;

        if(previousCount != count)
        {
            GetComponent<TextMeshProUGUI>().text = "x"+count;
            previousCount = count;
        }
    }
}
