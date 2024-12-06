using UnityEngine;

[CreateAssetMenu(menuName = "QuestScriptableObjects/TutorialObjective")]
public class TutorialObjective : ScriptableObject
{
    [Header("Objective Texts")]
    [TextArea] public string Title;
    [TextArea] public string TextOnThisObjective;
    [TextArea] public string TechnicalTExtOnThisObjective;
    
    public Objective tutorialObjective;
    public bool IsActive;
    public Transform MainTextPanelPosition;
    public Transform FunctionalTextPanelPosition;
}
