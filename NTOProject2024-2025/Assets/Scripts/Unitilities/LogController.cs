using System;
using UnityEngine;

public class LogController : MonoBehaviour
{
    [SerializeField] private bool _enabledLog;

    private void Awake()
    {
        Debug.unityLogger.logEnabled = _enabledLog;
    }
}
