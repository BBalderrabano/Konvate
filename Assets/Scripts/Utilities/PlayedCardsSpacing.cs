using UnityEngine;
using UnityEngine.UI;

public class PlayedCardsSpacing : MonoBehaviour
{
    int childAmount = 0;
    HorizontalLayoutGroup layoutGroup;

    private void Start()
    {
        layoutGroup = GetComponent<HorizontalLayoutGroup>();
    }

    void Update()
    {
        int childCount = transform.GetComponentsInChildren<CardInstance>().Length;

        if (childAmount != childCount)
        {
            if (childCount <= 2)
            {
                layoutGroup.spacing = -800;
            }
            else if(childCount <= 3)
            {
                layoutGroup.spacing = -600;
            }
            else if(childCount <= 4)
            {
                layoutGroup.spacing = -140;
            }
            else if(childCount > 4)
            {
                layoutGroup.spacing = 0;
            }

            childAmount = childCount;
        }
    }
}
