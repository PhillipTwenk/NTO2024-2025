using System;
using UnityEngine;

public class EntryLocationTriggerHandler : MonoBehaviour
{
    public GameEvent MusicEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            MusicEvent.TriggerEvent();
        }
    }
}
