using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static Player;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public Player player;
    public Enemy enemy;
    public Text resultText;

    private Player.Choice pendingPlayerChoice = Player.Choice.None;
    private Coroutine countdownCoroutine;

    void StartNewRound()
    {
        player.ResetToDefault();
        enemy.ResetToDefault();
        resultText.text = "";

        pendingPlayerChoice = Player.Choice.None;
        playerCanPick = true;
        countdownCoroutine = StartCoroutine(PlayerPickCountdown());
    }

    IEnumerator PlayerPickCountdown()
    {
        float timer = 3f;
        while (timer > 0f)
        {
            resultText.text = "Pick in: " + Mathf.Ceil(timer);
            timer -= Time.deltaTime;
            yield return null;
        }

        playerCanPick = false;

        // Show the player's choice now
        if (pendingPlayerChoice == Player.Choice.None)
        {
            player.SetChoice(Player.Choice.None); // automatic loss
            Debug.Log("[GameManager] Player didn't pick - automatic loss.");
        }
        else
        {
            player.SetChoice(pendingPlayerChoice);
        }

        void Awake()
    {
        // simple singleton guard
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
        {
            StartNewRound();
        }

         // Enemy picks after player time is up
        enemy.PickRandomChoice();

        // Determine winner
        string result = DetermineWinner(player.CurrentChoice, enemy.CurrentChoice);
        if (resultText != null) resultText.text = result;

        // Wait 2 seconds then start next round
        yield return new WaitForSeconds(2f);
        StartNewRound();
    }

    public bool CanPlayerPick()
    {
        return playerCanPick;
    }

    string DetermineWinner(Player.Choice playerChoice, Player.Choice enemyChoice)
    {
        if (playerChoice == enemyChoice) return "Draw!";
        if ((playerChoice == Player.Choice.Rock && enemyChoice == Player.Choice.Scissors) ||
            (playerChoice == Player.Choice.Paper && enemyChoice == Player.Choice.Rock) ||
            (playerChoice == Player.Choice.Scissors && enemyChoice == Player.Choice.Paper))
            return "Player Wins!";
        return "Enemy Wins!";
    }

    // Called by Player when they finish input
    public void PlayerHasChosen(Player.Choice choice)
    {
        if (!playerCanPick)
            return; // ignore input after time is up

        player.SetChoice(choice);
        player.SetSprite(choice);
        playerCanPick = false;

        if (player.CurrentChoice == Player.Choice.None)
        {
            player.SetChoice(Player.Choice.None); // default sprite
            Debug.Log("[GameManager] Player didn't pick - automatic loss.");
        }

        // Enemy picks after player time is up
        enemy.PickRandomChoice();

        string result = DetermineWinner(player.CurrentChoice, enemy.CurrentChoice);
        if (resultText != null) resultText.text = result;

        // Wait 2 seconds then start next round
        StartCoroutine(WaitAndStartNextRound(2f));
    }

    IEnumerator WaitAndStartNextRound(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartNewRound();
    }
}