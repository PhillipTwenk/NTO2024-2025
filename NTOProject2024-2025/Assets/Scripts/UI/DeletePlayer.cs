using TMPro;
using UnityEngine;

public class DeletePlayer : MonoBehaviour
{
    [SerializeField] private string DefaultPlayername;
    [SerializeField] private EntityID playerID;
    [SerializeField] private TextMeshProUGUI textNewCharacter;
    [SerializeField] private TextMeshProUGUI textPlayerName;

    /// <summary>
    /// Нажатие на кнопку удаления игрока
    /// </summary>
    public async void ClickDeletePlayerButton()
    {
        textNewCharacter.gameObject.SetActive(true);
        textNewCharacter.text = DefaultPlayername;
        textPlayerName.gameObject.SetActive(false);

        _ = APIManager.Instance.DeletePlayer(playerID.Name);
        playerID.Name = "None";
        
        gameObject.SetActive(false);
    }
}
