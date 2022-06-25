using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : Button, IPointerEnterHandler, IPointerExitHandler
{

    private ToolTip toolTip;
    protected override void Awake()
    {
        Transform t = transform.Find("ToolTip");
        if (t != null)
        {
            toolTip = t.GetComponent<ToolTip>();
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (toolTip != null) toolTip.OpenAnimation();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (toolTip != null) toolTip.CloseAnimation();
    }
}
