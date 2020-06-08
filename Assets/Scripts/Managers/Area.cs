using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public AreaLogic logic;

    public void OnDrop(Vector3 position)
    {
        logic.Execute(position);
    }
}
