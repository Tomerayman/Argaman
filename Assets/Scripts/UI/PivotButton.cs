using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotButton : MonoBehaviour
{
    public PivotPoint p;

    protected virtual void Awake()
    {
        p.buttons.Add(this);
    }

    protected virtual void OnMouseDown()
    {
        if (p != null) p.OnClick();
    }
}
