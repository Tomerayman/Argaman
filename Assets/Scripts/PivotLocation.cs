using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotLocation : PivotPoint
{
    public LocationUI locationUI;

    protected override void Awake()
    {
        base.Awake();
        onFinishEnter.AddListener(() => SetLocationUI(true));
        onExit.AddListener((s) => SetLocationUI(false));
    }

    protected override void Start()
    {
        base.Start();
        locationUI.gameObject.SetActive(false);
    }

    public override void OnClick()
    {
        base.OnClick();
        CameraRotation.Instance.IsLocked = true;
    }

    public void SetLocationUI(bool open)
    {
        StartCoroutine(TimedOpenClose());

        IEnumerator TimedOpenClose()
        {
            if (open) locationUI.gameObject.SetActive(true);
            locationUI.SetMainPanel(open);
            if (!open)
            {
                yield return new WaitForSeconds(2f);
                locationUI.gameObject.SetActive(false);
            }
        }
    }
    


}
