using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class APIRsControlInChoicePlayerPanel : MonoBehaviour
{
    [SerializeField] private string DefaultPlayername; 
    [SerializeField] private EntityID player;
    [SerializeField] private TextMeshProUGUI textNewCharacter;
    [SerializeField] private TextMeshProUGUI textPlayerName;

    /// <summary>
    /// Изменение текста при включении
    /// </summary>
    private void OnEnable()
    {
        if (player.Name == player.DefaultName)
        {
            textNewCharacter.gameObject.SetActive(true);
            textNewCharacter.text = DefaultPlayername;
            textPlayerName.gameObject.SetActive(false);
        }
        else
        {
            textPlayerName.gameObject.SetActive(true);
            textPlayerName.text = player.Name;
            textNewCharacter.gameObject.SetActive(false);
        }
    }
}
