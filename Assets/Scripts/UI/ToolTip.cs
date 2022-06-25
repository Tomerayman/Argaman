using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

public class ToolTip : MonoBehaviour
{
    ProceduralImage mask;
    RectTransform rect;
    ProceduralImage image;
    TMP_Text text;
    public bool IsShowing;
    public bool IsMidAnimation;
    public float animationSpeed = 0.5f;

    private float originWidth;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        originWidth = rect.rect.width;
        image = rect.Find("Image").GetComponent<ProceduralImage>();
        RectTransform imRect = image.GetComponent<RectTransform>();
        imRect.anchorMin = new Vector2(1, 0.5f);
        imRect.anchorMax = new Vector2(1, 0.5f);
        imRect.pivot = new Vector2(1, 0.5f);
        rect.sizeDelta = new Vector2(0, rect.rect.height);
    }

    public void OpenAnimation()
    {
        IsShowing = true;
        Vector2 openSize = new Vector2(originWidth, rect.rect.height);
        rect.DOSizeDelta(openSize, animationSpeed).OnComplete(() =>
        {
            if (!IsShowing) CloseAnimation();
        });
    }

    public void CloseAnimation()
    {
        IsShowing = false;
        Vector2 openSize = new Vector2(0, rect.rect.height);
        rect.DOSizeDelta(openSize, animationSpeed).OnComplete(() =>
        {
            if (IsShowing) OpenAnimation();
        });
    }


}
