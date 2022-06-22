using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLight : MonoBehaviour
{
    private List<MeshRenderer> meshs;
    private List<Material[]> materialsList, outlinesList;
    private Material[] materials, outlines;
    private bool isOn = false;
    // Start is called before the first frame update
    void Start()
    {
           meshs = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
           MeshRenderer main = GetComponent<MeshRenderer>();
           if (main)
               meshs.Add(main);
           materialsList = new List<Material[]>();
           outlinesList = new List<Material[]>();
           for(int j = 0; j< meshs.Count;j++)
           {
               materials = new Material[meshs[j].materials.Length];
               outlines = new Material[meshs[j].materials.Length];

               for (int i = 0; i < materials.Length; i++)
               {
                   Material outline = new Material(Shader.Find("AA/OutlinePulse"));
                   outline.SetTexture("_Diffuse", meshs[j].materials[i].mainTexture);
                   outline.SetFloat("_Outline_Width", 0);
                   outline.SetColor("_Outline_Color", new Color(1, 0.6f, 0, 1));
                   outline.SetFloat("_Pulse_Speed", 0);
                   outline.SetFloat("_Fresnel_Width", 0.5f);
                   outline.SetFloat("_Corrective_Glow",-0.7f);
                   outline.SetFloat("_Emission_Intensity", 0.2f);

                   materials[i] = meshs[j].materials[i];
                   outlines[i] = outline;
               }
               materialsList.Add(materials);
               outlinesList.Add(outlines);
           }
           SetOutline(isOn);

    }

    public void SetOutline(bool flage)
    {
        isOn = flage;
        if (flage)
            Show();
        else
            Hide();
    }

    public void Hide()
    {
        if (meshs == null)
            return;
        for (int j = 0; j < meshs.Count; j++)
        {
            meshs[j].materials = materialsList[j];
        }
    }

    public void Show()
    {
        if (meshs == null)
            return;
        for (int j = 0; j < meshs.Count; j++)
        {
            meshs[j].materials = outlinesList[j];
        }
    }
}