using HighlightPlus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
    private List<MeshRenderer> meshs;
    private List<Material[]> materialsList, outlinesList;
    private Material[] materials ,outlines;
    private bool isOn = false;
    private HighlightEffect highlight;
    
    void Awake()
    {
        highlight = gameObject.GetComponent<HighlightEffect>();
        if (highlight == null) 
        {
            highlight = gameObject.AddComponent<HighlightEffect>();
            highlight.outlineColor = Color.yellow;
            highlight.overlay = 0;
            highlight.overlayMinIntensity = 0;

        }
        highlight.seeThrough = SeeThroughMode.Never;
        highlight.outlineVisibility = Visibility.AlwaysOnTop;
        highlight.outlineWidth = 0.5f;
        highlight.glow = 0f;
        highlight.glowWidth = 0f;
        highlight.glowAnimationSpeed = 0.5f;
    }

    public void SetOutline(bool flag)
    {
        isOn = flag;
        if (flag)
            Show();
        else
            Hide();
    }

    public void Hide()
    {
        /*  if (meshs == null)
              return;
          for (int j = 0; j < meshs.Count; j++)
          {
              meshs[j].materials = materialsList[j];
          }*/
        if (highlight == null)
            return;

        highlight.SetHighlighted(false);

    }

    public void Show()
    {
        if (highlight == null)
            return;
        highlight.SetHighlighted(true);
      /*  if (meshs == null)
            return;
        for (int j = 0; j < meshs.Count; j++)
        {
            meshs[j].materials = outlinesList[j];
        }*/
    }

    public void SetIncludeChildren(bool include)
    {
        highlight.effectGroup = include ? TargetOptions.Children : TargetOptions.OnlyThisObject;
    }

    private void OnDisable()
    {
        StopFlickering();
    }
    internal void Flickering()
    {
        if(gameObject.activeInHierarchy)
            StartCoroutine(StartFlickering());
    }
    internal void StopFlickering()
    {
        StopAllCoroutines();
        Hide();
    }
    private IEnumerator StartFlickering()
    {
        while(true)
        {
            Show();
            yield return new WaitForSeconds(1);
            Hide();
            yield return new WaitForSeconds(0.3f);
        }
    }


}
