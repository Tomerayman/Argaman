using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotObject : ArgButton
{
    protected Outline outline;

    protected override void Awake()
    {
        pivotPoint.mainButton = this;
        outline = gameObject.GetComponent<Outline>();
        if (outline == null) outline = gameObject.AddComponent<Outline>();
    }

    protected override void Start()
    {
        gameObject.SetActive(pivotPoint.isActiveAndEnabled);
    }

    protected override void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        outline.Show();
    }

    private void OnMouseExit()
    {
        outline.Hide();
    }

    private void OnMouseDown()
    {
        pivotPoint.OnClick();
    }
}
