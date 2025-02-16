using System;
using System.Collections;
using UnityEngine;

public static class Utility
{
    /// <summary>
    /// Вызов метода через указанное количество времени
    /// </summary>
    /// <param name="mb"> Ссылка на MonoBehaviour скрипта</param>
    /// <param name="f"> Делегат типа Action в который загружается метод </param>
    /// <param name="delay"> Время, через которое нужно вызвать метод </param>
    public static void Invoke(MonoBehaviour mb, Action f, float delay)
    {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    private static IEnumerator InvokeRoutine(Action f, float delay)
    {
        yield return new WaitForSeconds(delay);
        f();
    }
}