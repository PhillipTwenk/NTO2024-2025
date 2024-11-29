using UnityEngine;
using UnityEngine.UI;

public class Test5 : MonoBehaviour
{
    public RectTransform questInfo; // RectTransform QuestInfo
    public RectTransform content;  // RectTransform Content

    private void Update()
    {
        // Принудительное обновление макета для QuestInfo
        LayoutRebuilder.ForceRebuildLayoutImmediate(questInfo);

        // Принудительное обновление макета для Content
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
    }
}
