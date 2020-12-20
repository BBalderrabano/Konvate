using UnityEngine;
using TMPro;

public class CountChips : MonoBehaviour
{
    public Transform checkFrom;
    public int max = 999;

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
            GetComponent<TextMeshProUGUI>().text = "x" + Mathf.Min(count, max);
            previousCount = count;
        }
    }
}
