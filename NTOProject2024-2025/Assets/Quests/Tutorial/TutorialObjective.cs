 using UnityEngine;

[CreateAssetMenu(menuName = "QuestScriptableObjects/TutorialObjective")]
public class TutorialObjective : ScriptableObject
{
    [TextArea] public string TextOnThisObjective;
    public Objective tutorialObjective;
    public bool IsActive;
    public Transform MainTextPanelPosition;
    public Transform FunctionalTextPanelPosition;
}
