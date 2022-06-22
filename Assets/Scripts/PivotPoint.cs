using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotPoint : MonoBehaviour
{
    public Vector2 xRange = new Vector2(-360, 360);
    public Vector2 yRange = new Vector2(-360, 360);
    public Vector3 offset;
    public GameObject spawnPoint;
    public List<PivotButton> buttons;
    public GameObject ui;
    private Quaternion _originRotation;


    private void Awake()
    {
        _originRotation = transform.rotation;
        ui.SetActive(false);
    }

    void Start()
    {
        spawnPoint.SetActive(false);
    }

    public void OnClick()
    {
        CameraRotation.Instance.OnClickPivotPoint(this, false);
    }

    public void Reset()
    {
        transform.rotation = _originRotation;
        SetActive(false);
    }

    public void SetActive(bool active)
    {
        ui.SetActive(active);
        buttons.ForEach(b => b.gameObject.SetActive(!active));
    }

}
