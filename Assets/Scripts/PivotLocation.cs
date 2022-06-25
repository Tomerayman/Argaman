using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotLocation : PivotPoint
{
    public LocationUI locationUI;

    protected override void Awake()
    {
        base.Awake();
        onFinishEnter.AddListener(() => locationUI.SetMainPanel(true));
        onExit.AddListener((s) => locationUI.SetMainPanel(false));
    }

    public override void OnClick()
    {
        base.OnClick();
        CameraRotation.Instance.IsLocked = true;
    }
    


}
