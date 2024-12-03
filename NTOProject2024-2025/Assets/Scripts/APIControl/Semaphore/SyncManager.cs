using System.Threading;
using UnityEngine;

public class SyncManager : MonoBehaviour
{
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1); 

    public static SemaphoreSlim Semaphore => _semaphore; 
}
