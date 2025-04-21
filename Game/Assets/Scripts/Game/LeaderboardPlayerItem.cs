using TMPro;
using UnityEngine;

public class LeaderboardPlayerItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI slText;
    [SerializeField] private TextMeshProUGUI userIdText;
    [SerializeField] private TextMeshProUGUI winText;

    public void InitInfo(string sl, string userId, string win)
    {
        slText.text = sl;
        userIdText.text = userId;
        winText.text = win;
    }
}
