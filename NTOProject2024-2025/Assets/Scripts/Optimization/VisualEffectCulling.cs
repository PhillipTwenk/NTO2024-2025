using UnityEngine;
using UnityEngine.VFX;

public class VisualEffectCulling : MonoBehaviour
{
    public VisualEffect visualEffect;
    public Camera mainCamera;

    void Update()
    {
        if (IsEffectVisible())
        {
            if (!visualEffect.enabled)
                visualEffect.enabled = true;
        }
        else
        {
            if (visualEffect.enabled)
                visualEffect.enabled = false;
        }
    }

    bool IsEffectVisible()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}