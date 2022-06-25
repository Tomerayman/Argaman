using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class ArgButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected PivotPoint pivotPoint;
    [SerializeField] protected float duration = 1;
    [SerializeField] protected float maxScale = 1.2f;
    [SerializeField] protected ProceduralImage buttonOutline;
    protected float borderWidth;
    protected Camera cam;
    protected Button button;
    protected ToolTip toolTip;

    protected virtual void Awake()
    {
        pivotPoint.mainButton = this;
        borderWidth = buttonOutline.BorderWidth;
        button = GetComponent<Button>();
        button.onClick.AddListener(pivotPoint.OnClick);
        Transform t = transform.Find("ToolTip");
        if (t != null)
        {
            toolTip = t.GetComponent<ToolTip>();
        }
        
    }

    protected virtual void Start()
    {

        cam = Camera.main;
        Beat();
        gameObject.SetActive(pivotPoint.isActiveAndEnabled);
    }

    protected virtual void Update()
    {
        Vector3 v = cam.WorldToScreenPoint(pivotPoint.mainButtonPlace.position);
        if (v.z > 0) transform.position = v;
    }


    protected virtual void Beat()
    {
        ScaleUp();

        void ScaleUp()
        {
            buttonOutline.transform.DOScale(maxScale, duration).SetEase(Ease.InOutSine)
                .OnUpdate(()=>
                    {
                        buttonOutline.BorderWidth = borderWidth / Mathf.Pow(buttonOutline.transform.localScale.x, 5);
                    })
                .OnComplete(ScaleDown);
        }
        
        void ScaleDown()
        {
            buttonOutline.transform.DOScale(1, duration).SetEase(Ease.InOutSine)
                .OnUpdate(() =>
                    {
                        buttonOutline.BorderWidth = borderWidth / Mathf.Pow(buttonOutline.transform.localScale.x, 5);
                    })
                .OnComplete(ScaleUp);
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (toolTip != null) toolTip.OpenAnimation();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (toolTip != null) toolTip.CloseAnimation();
    }
}
