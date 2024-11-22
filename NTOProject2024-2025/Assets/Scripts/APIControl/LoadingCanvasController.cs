using System;
using UnityEngine;

public class LoadingCanvasController : MonoBehaviour
{
    public static LoadingCanvasController Instance { get; private set; }

    public GameObject LoadingCanvasNotTransparent;
    public GameObject LoadingCanvasTransparent;

    private void Awake()
    {
        Instance = this;
    }
}
