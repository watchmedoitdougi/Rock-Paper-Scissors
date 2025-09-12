using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public Player player;
    public Enemy enemy;
    public Text resultText;

    void Awake()
    {
        // simple singleton guard
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[GameManager] Another instance exists - destroying this one.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // sanity checks
        if (player == null) Debug.LogError("[GameManager] Player reference not assigned in Inspector.");
        if (enemy == null) Debug.LogError("[GameManager] Enemy reference not assigned in Inspector.");

        StartNewRound();
    }

    // Called by Player when they finish their input
    public void PlayerHasChosen(Player.Choice playerChoice)
    {
        if (player == null || enemy == null)
        {
            Debug.LogError("[GameManager] Player or Enemy is null. Can't resolve round.");
            return;
        }

        // Enemy picks now (you can also pick at round start if preferred)
        enemy.PickRandomChoice();

        string result = DetermineWinner(playerChoice, enemy.CurrentChoice);

        if (resultText != null)
            resultText.text = result;
        else
            Debug.Log("[GameManager] Result: " + result);

        // After 2 seconds reset both sprites to their defaults
        Invoke(nameof(StartNewRound), 2f);
    }

    // Reset sprites & clear text, then get ready for next round
    void StartNewRound()
    {
        if (player != null) player.ResetToDefault();
        if (enemy != null) enemy.ResetToDefault();
        if (resultText != null) resultText.text = "";
    }

    string DetermineWinner(Player.Choice playerChoice, Player.Choice enemyChoice)
    {
        if (playerChoice == enemyChoice) return "Draw!";

        if ((playerChoice == Player.Choice.Rock && enemyChoice == Player.Choice.Scissors) ||
            (playerChoice == Player.Choice.Paper && enemyChoice == Player.Choice.Rock) ||
            (playerChoice == Player.Choice.Scissors && enemyChoice == Player.Choice.Paper))
        {
            return "Player Wins!";
        }

        return "Enemy Wins!";
    }
}