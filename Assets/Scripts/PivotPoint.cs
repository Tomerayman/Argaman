using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PivotPoint : MonoBehaviour
{
    public static List<PivotPoint> pivots = new List<PivotPoint>();
    public static int currOrder = 0;
    public int order;
 public Vector2 xRange = new Vector2(-360, 360);
    public Vector2 yRange = new Vector2(-360, 360);
    public Vector3 offset;
    public GameObject spawnPoint;
    [NonSerialized] public ArgButton mainButton;
    public Transform mainButtonPlace;
    public List<PivotButton> buttons;
    public GameObject ui;
    public GameObject subItems;
    public UnityEvent<string> onEnter = new UnityEvent<string>();
    public UnityEvent<string> onExit = new UnityEvent<string>();
    protected Quaternion _originRotation;
    [NonSerialized] public Transform axis;
    [NonSerialized] public UnityEvent onFinishEnter;


    protected virtual void Awake()
    {
        onFinishEnter = new UnityEvent();
        _originRotation = transform.rotation;
        axis = new GameObject("axis").transform;
        axis.parent = transform;
        axis.localPosition = Vector3.zero;
        axis.rotation = transform.rotation;
        pivots.Add(this);
    }

    protected virtual void Start()
    {
        spawnPoint.SetActive(false);
        mainButtonPlace.gameObject.SetActive(false);
        if (ui != null) ui.SetActive(false);
        SetSubItems(false);
    }

    public virtual void OnClick()
    {
        CameraRotation.Instance.OnClickPivotPoint(this, false);
    }

    public static void SetActiveByOrder(int currOrder)
    {
        foreach (PivotPoint p in pivots)
        {
            if (p.order > 2) continue;
                p.SetVisible(p.order == currOrder + 1);
        }
    }

    public void Reset()
    {
        axis.rotation = _originRotation;
        SetActive(false);
    }

    public void SetSubItems(bool active)
    {
        if (subItems != null)
        {
            foreach (Transform subPivot in subItems.transform)
            {
                PivotPoint p = subPivot.GetComponent<PivotPoint>();
                p.mainButton.gameObject.SetActive(active);
            }
        }
    }

    public virtual void SetVisible(bool vis)
    {
        if (mainButton != null) mainButton.gameObject.SetActive(vis);
    }

    public virtual void SetActive(bool active, bool init=false)
    {
        if (ui != null) ui.SetActive(active);
        SetVisible(!active);
        if (active)
        {
            onEnter.Invoke(name);
            SetActiveByOrder(order);
        }
        else if (!init) onExit.Invoke(name);

    }

}
