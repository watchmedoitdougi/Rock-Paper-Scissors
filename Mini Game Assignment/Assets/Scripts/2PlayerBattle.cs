using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class Battle2Player : MonoBehaviour
{
    public TMP_Text rockText;
    public TMP_Text paperText;
    public TMP_Text scissorsText;
    public TMP_Text shootText;
    public TMP_Text resultText;

    public Button bestOf3Button;
    public Button mainMenuButton;

    public PlayerBattle player1;      // WASD
    public Enemy player2;       // Arrow keys

    private bool inputLocked = false;

    private int player1Wins = 0;
    private int player2Wins = 0;
    private int roundCount = 0;

    void Start()
    {
        bestOf3Button.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
        StartCoroutine(StartBattleRound());
    }

    IEnumerator StartBattleRound()
    {
        // Reset and prepare
        inputLocked = false;
        player1.ResetSprite();
        player2.ResetSprite();
        player1.player1Choice = "None";
        player2.player2Choice = "None";
        resultText.text = "";

        // Hide all countdown texts
        rockText.gameObject.SetActive(false);
        paperText.gameObject.SetActive(false);
        scissorsText.gameObject.SetActive(false);
        shootText.gameObject.SetActive(false);

        // Countdown sequence
        rockText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        rockText.gameObject.SetActive(false);

        paperText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        paperText.gameObject.SetActive(false);

        scissorsText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        scissorsText.gameObject.SetActive(false);

        shootText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        shootText.gameObject.SetActive(false);

        // Give both players 1 second to choose
        yield return new WaitForSeconds(1);

        // Lock input and evaluate
        inputLocked = true;

        string p1Choice = player1.player1Choice;
        string p2Choice = player2.player2Choice;

        // Reveal choices visually
        player1.ShowChoice(p1Choice);
        player2.ShowChoice(p2Choice);

        // Determine results
        if (p1Choice == "None" && p2Choice == "None")
        {
            EndBattleRound("Nobody chose!");
        }
        else if (p1Choice == "None")
        {
            player2Wins++;
            EndBattleRound("Player 2 Wins by default!");
        }
        else if (p2Choice == "None")
        {
            player1Wins++;
            EndBattleRound("Player 1 Wins by default!");
        }
        else
        {
            EvaluateWinner(p1Choice, p2Choice);
        }

        roundCount++;

        // After each round, show buttons
        bestOf3Button.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
    }

    void EvaluateWinner(string p1Choice, string p2Choice)
    {
        if (p1Choice == p2Choice)
        {
            EndBattleRound("Draw! Again!");
            return;
        }

        bool p1Wins =
            (p1Choice == "Rock" && p2Choice == "Scissors") ||
            (p1Choice == "Paper" && p2Choice == "Rock") ||
            (p1Choice == "Scissors" && p2Choice == "Paper");

        if (p1Wins)
        {
            player1Wins++;
            EndBattleRound("Player 1 Wins!");
        }
        else
        {
            player2Wins++;
            EndBattleRound("Player 2 Wins!");
        }
    }

    void EndBattleRound(string result)
    {
        resultText.text = result;

        if (player1Wins >= 2)
        {
            resultText.text = "Player 1 is the Champion!";
            mainMenuButton.gameObject.SetActive(true);
            bestOf3Button.gameObject.SetActive(false);
        }
        else if (player2Wins >= 2)
        {
            resultText.text = "Player 2 is the Champion!";
            mainMenuButton.gameObject.SetActive(true);
            bestOf3Button.gameObject.SetActive(false);
        }
        else if (result.Contains("Draw"))
        {
            StartCoroutine(RestartAfterDelay());
        }
    }

    IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(StartBattleRound());
    }

    public void PlayBestOf3()
    {
        bestOf3Button.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);

        if (roundCount < 3)
        {
            StartCoroutine(StartBattleRound());
        }
        else
        {
            // Reset everything for a new best-of-3 session
            roundCount = 0;
            player1Wins = 0;
            player2Wins = 0;

            StartCoroutine(StartBattleRound());
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}