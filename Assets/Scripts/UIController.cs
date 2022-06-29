using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject mainViewsButtons;
    public GameObject headerButtons;

    public void SetMainViewsButtons(bool active)
    {
        mainViewsButtons.SetActive(active);
    }
}
