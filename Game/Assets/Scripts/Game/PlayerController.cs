using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("Player Info")]
    public string playerId = "A"; // Set to "A" or "B" in Inspector

    private int realRoll;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ReceiveRoll(int number)
    {
        realRoll = number;
        UIManager.Instance.UpdateRoll(playerId, number);
    }

    public void ClaimNumber(int claimed)
    {
        GameManager.Instance.SendClaim(playerId, claimed);
    }

    public void MakeDecision(bool believe)
    {
        GameManager.Instance.SendDecision(playerId, believe ? NetworkConstants.Believe : NetworkConstants.Bluff);
    }

    public int GetRealRoll()
    {
        return realRoll;
    }
}