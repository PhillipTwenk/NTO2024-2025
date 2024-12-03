using System;
using UnityEngine;

public class SelectThisButtonMainMenu : MonoBehaviour
{
    public GameObject indicator;

    private void OnEnable()
    {
        indicator.SetActive(false);
    }

    public void OnMouseEnterButton()
    {
        indicator.SetActive(true);
    }

    public void OnMouseExitButton()
    {
        indicator.SetActive(false);
    }
}
