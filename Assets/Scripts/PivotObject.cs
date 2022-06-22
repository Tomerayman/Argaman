using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotObject : PivotButton
{
    private Outline outline;

    protected override void Awake()
    {
        outline = gameObject.AddComponent<Outline>();
    }

    private void OnMouseEnter()
    {
        outline.Show();
    }

    private void OnMouseExit()
    {
        outline.Hide();
    }
}
