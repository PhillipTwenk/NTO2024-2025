using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static bool IsTutorialActive;
    private bool CanEnterM;
    private bool CanEnterT;
    private Action TextUpdateEvent;
    public static Action UpdateTutorialStateEvent;
    private TutorialObjective currentTutorialObjective;
    private int i;
    public static bool IsTutorialTimeStop;

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
        UpdateTutorialStateEvent += ()=> UpdateTutorialState();
    }

    private void Update()
    {
        if (Input.GetButtonDown("TutorialUpdate") && IsTutorialActive && CanEnterM && currentTutorialObjective.IsAutomaticalyEnterOnThisStep)
        {
            if (currentTutorialObjective.IsTechPanelActiveOnThisStep)
            {
                if (CanEnterT)
                {
                    Debug.Log("SkibidiINC");
                    UpdateTutorialStateEvent?.Invoke();
                }
            }
            else
            {
                Debug.Log("SkibidiINC");
                UpdateTutorialStateEvent?.Invoke();
            }
        }
    }

    public void UpdateTutorialState()
    {
        Debug.Log("SKIBIDIDOPDOPDOPDOPDOPDOP");
        i++;
        if (i == 33)
        {
            EndTutorial();
        }
        if ((i - 1) >= 0)
        {
            currentTutorialObjective.IsActive = false;
        }
        currentTutorialObjective = TutorialObjectives[i];
        currentTutorialObjective.IsActive = true;
        
        TextUpdateEvent?.Invoke();
    }

    public void StartTutorial()
    {
        Debug.Log("Туториал начат");
        i = 0;
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
        Debug.Log("Панель обновляется");
        CanEnterM = false;
        CanEnterT = false;
        TitleText.text = currentTutorialObjective.Title;
        
        if (currentTutorialObjective.IsTimeStopOnThisStep)
        {
            FadeFone.SetActive(true);
            Time.timeScale = 0f;
            IsTutorialTimeStop = true;
        }
        else
        {
            FadeFone.SetActive(false);
            Time.timeScale = 1f;
            IsTutorialTimeStop = false;
        }
        
        if (UIPanel == TechnicalUIPanel)
        {
            if (currentTutorialObjective.IsTechPanelActiveOnThisStep)
            {
                UIPanel.transform.parent.gameObject.SetActive(true);
                UIPanel.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition  =
                    PositionTypes[currentTutorialObjective.TechUIPositionTypeOnThisStep];
                await TextWriter(UIPanel, currentTutorialObjective.TechnicalTExtOnThisObjective);
                CanEnterT = true;

            }
            else
            {
                UIPanel.transform.parent.gameObject.SetActive(false);
            }
        }
        else
        {
            UIPanel.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition  =
                PositionTypes[currentTutorialObjective.MainUIPositionTypeOnThisStep];
            await TextWriter(UIPanel, currentTutorialObjective.TextOnThisObjective);
            CanEnterM = true;
        }
        
        
    }

    private async Task TextWriter(TextMeshProUGUI UIPanel, string Text)
    {
        string textBuffer = null;
        UIPanel.text = textBuffer;
        foreach (var c in Text)
        {
            textBuffer += c;
            UIPanel.text = textBuffer;
            await Task.Delay((int)(1000 / charactersPerSecond));
        }

        Debug.Log("Текст полностью напечатан");
    }
    
    
}
