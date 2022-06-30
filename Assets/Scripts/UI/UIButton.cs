using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : Button, IPointerEnterHandler, IPointerExitHandler
{

    public Sprite hoverSprite;
    private ToolTip toolTip;
    private Sprite normalSprite;

    protected override void Awake()
    {
        base.Awake();
        Transform t = transform.Find("ToolTip");
        if (t != null)
        {
            toolTip = t.GetComponent<ToolTip>();
        }
        normalSprite = targetGraphic.GetComponent<Image>().sprite;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (toolTip != null) toolTip.OpenAnimation();
        if (hoverSprite != null) targetGraphic.GetComponent<Image>().sprite = hoverSprite;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (toolTip != null) toolTip.CloseAnimation();
        if (hoverSprite != null) targetGraphic.GetComponent<Image>().sprite = normalSprite;
    }
}
