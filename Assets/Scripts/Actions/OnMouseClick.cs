using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "Actions/On Mouse Click")]
public class OnMouseClick : PlayerAction
{
    public override void Execute(float d)
    {
        if (Input.GetMouseButtonDown(0))
        {
            List<RaycastResult> results = Settings.GetUIObjects();

            IClickable c = null;

            foreach (RaycastResult r in results)
            {
                c = r.gameObject.GetComponentInParent<IClickable>();

                if (c != null)
                {
                    c.OnClick();
                    break;
                }
            }
        }
    }
}
