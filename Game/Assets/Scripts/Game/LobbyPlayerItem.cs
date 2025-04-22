using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userIdText;
    [SerializeField] private Button challengeButton;

    public void InitInfo(string userId)
    {
        userIdText.text = userId;
    }
}
