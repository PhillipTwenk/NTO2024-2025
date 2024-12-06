using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static bool IsTutorialActive;
    private bool CanEnter;
    private Action TextUpdateEvent;
    public static Action UpdateTutorialStateEvent;
    private TutorialObjective currentTutorialObjective;
    private int i;

    [Header("Quest Info")]
    public List<TutorialObjective> TutorialObjectives;
    public QuestController BearQC;
    private QuestOwner questOwner;

    [Header("UI elements")] 
    public TextMeshProUGUI MainUIPanel;
    public TextMeshProUGUI TechnicalUIPanel;
    public TextMeshProUGUI TitleText;
    public GameObject FadeFone;

    [Header("Parameters")] 
    public float charactersPerSecond;
    public List<Vector3> PositionTypes;
    

    private void Awake()
    {
        questOwner = GetComponent<QuestOwner>();
        questOwner = GetComponent<QuestOwner>();
        UpdateTutorialStateEvent += ()=> UpdateTutorialState();
    }

    private void Update()
    {
        if (Input.GetButtonDown("TutorialUpdate") && IsTutorialActive && CanEnter && currentTutorialObjective.IsAutomaticalyEnterOnThisStep)
        {
            UpdateTutorialStateEvent?.Invoke();
        }
    }

    public void UpdateTutorialState()
    {
        if (i == 32)
        {
            EndTutorial();
        }
        i++;
        if ((i - 1) > 0)
        {
            currentTutorialObjective.IsActive = false;
        }
        currentTutorialObjective = TutorialObjectives[i];
        currentTutorialObjective.IsActive = true;
        
        TextUpdateEvent?.Invoke();
    }

    public void StartTutorial()
    {
        IsTutorialActive = true;
        questOwner.GiveQuest(BearQC);
        MainUIPanel.transform.parent.gameObject.SetActive(true);
        currentTutorialObjective = TutorialObjectives[i];
        TextUpdateEvent += () => UIPanelUpdate(MainUIPanel, currentTutorialObjective);
        TextUpdateEvent += () => UIPanelUpdate(TechnicalUIPanel, currentTutorialObjective);
        TextUpdateEvent?.Invoke();
        currentTutorialObjective.IsActive = true;
    }

    public void EndTutorial()
    {
        IsTutorialActive = false;
        MainUIPanel.transform.parent.gameObject.SetActive(false);
        TechnicalUIPanel.transform.parent.gameObject.SetActive(false);
        TextUpdateEvent -= () => UIPanelUpdate(MainUIPanel, currentTutorialObjective);
        TextUpdateEvent -= () => UIPanelUpdate(TechnicalUIPanel, currentTutorialObjective);
        currentTutorialObjective.IsActive = false;
    }

    private async void UIPanelUpdate(TextMeshProUGUI UIPanel, TutorialObjective currentTutorialObjective)         
    {
        CanEnter = false;
        TitleText.text = currentTutorialObjective.Title;
        
        if (currentTutorialObjective.IsTimeStopOnThisStep)
        {
            FadeFone.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            FadeFone.SetActive(false);
            Time.timeScale = 1f;
        }
        
        if (UIPanel == TechnicalUIPanel)
        {
            if (currentTutorialObjective.IsTechPanelActiveOnThisStep)
            {
                UIPanel.transform.parent.gameObject.SetActive(true);
                UIPanel.transform.parent.transform.position =
                    PositionTypes[currentTutorialObjective.TechUIPositionTypeOnThisStep];
                await TextWriter(UIPanel, currentTutorialObjective.TechnicalTExtOnThisObjective);
            }
            else
            {
                UIPanel.transform.parent.gameObject.SetActive(false);
            }
        }
        else
        {
            UIPanel.transform.parent.transform.position =
                PositionTypes[currentTutorialObjective.MainUIPositionTypeOnThisStep];
            await TextWriter(UIPanel, currentTutorialObjective.TextOnThisObjective);
        }
        
        
    }

    private async Task TextWriter(TextMeshProUGUI UIPanel, string Text)
    {
        string textBuffer = null;
        UIPanel.text = textBuffer;
        foreach (var c in Text)
        {
            textBuffer += c;
            UIPanel.text += textBuffer;
            await Task.Delay((int)(1000 / charactersPerSecond));
        }

        CanEnter = true;
    }
    
    
}
