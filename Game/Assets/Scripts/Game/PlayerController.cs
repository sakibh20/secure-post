using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("Player Info")]
    public string playerId = "A"; // Set to "A" or "B" in Inspector

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    //
    // public void ReceiveRoll(int number)
    // {
    //     UIManager.Instance.UpdateRoll(playerId, number);
    // }
}