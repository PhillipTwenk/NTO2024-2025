using System;
using UnityEngine;

public class EntryPointMainMenu : MonoBehaviour
{
    [SerializeField] private EntityID player1;
    [SerializeField] private EntityID player2;
    [SerializeField] private EntityID player3;
    
    [SerializeField] private bool IsInEditor;
    private void Start()
    {
        //TestInEditor
        if (IsInEditor)
        {
            player1.DefaultRevert();
            player2.DefaultRevert();
            player3.DefaultRevert();
        }

        InitializeData();
    }
    
    private void InitializeData()
    {
        
    }
}
