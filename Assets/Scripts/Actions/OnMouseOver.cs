using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "Actions/On Mouse Over")]
public class OnMouseOver : PlayerAction
{
    public override void Execute(float d)
    {
        if (Input.GetMouseButton(0))
        {
            List<RaycastResult> results = Settings.GetUIObjects();

            IClickable c = null;

            foreach (RaycastResult r in results)
            {
                c = r.gameObject.GetComponentInParent<IClickable>();

                if (c != null)
                {
                    c.OnHighlight();
                    break;
                }
            }
        }
    }
}
