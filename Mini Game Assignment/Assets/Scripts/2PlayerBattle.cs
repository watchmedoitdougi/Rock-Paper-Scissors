using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class Battle2Player : MonoBehaviour
{
    public SpriteRenderer rockimage;
    public SpriteRenderer paperimage;
    public SpriteRenderer scissorsimage;
    public SpriteRenderer shootimage;
    public TMP_Text resultText;

    public Button bestOf3Button;
    public Button mainMenuButton;

    public PlayerBattle player1;   // WASD
    public Enemy player2;          // Arrow Keys or AI

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
        inputLocked = false;

        // Reset sprites + choices
        player1.ResetSprite();
        player2.ResetSprite();

        resultText.text = "";

        rockimage.gameObject.SetActive(false);
        paperimage.gameObject.SetActive(false);
        scissorsimage.gameObject.SetActive(false);
        shootimage.gameObject.SetActive(false);

        // Countdown visuals
        rockimage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        rockimage.gameObject.SetActive(false);

        paperimage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        paperimage.gameObject.SetActive(false);

        scissorsimage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        scissorsimage.gameObject.SetActive(false);

        shootimage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        shootimage.gameObject.SetActive(false);

        // 1 second to input
        yield return new WaitForSeconds(1);

        // Lock input so choices freeze
        inputLocked = true;

        Choice p1 = player1.choice;
        Choice p2 = player2.choice;

        // Reveal choices (they already set their sprites when SetChoice was called)
        // If neither picked:
        if (p1 == Choice.None && p2 == Choice.None)
        {
            EndBattleRound("Nobody chose!");
        }
        else if (p1 == Choice.None)
        {
            player2Wins++;
            player1.ShowLosingSprite(Choice.None);
            EndBattleRound("Player 2 Wins by default!");
        }
        else if (p2 == Choice.None)
        {
            player1Wins++;
            player2.ShowLosingSprite(Choice.None);
            EndBattleRound("Player 1 Wins by default!");
        }
        else
        {
            EvaluateWinner(p1, p2);
        }

        roundCount++;

        bestOf3Button.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
    }

    void Update()
    {
        if (inputLocked) return;

        // Player 1 input (WASD)
        if (Input.GetKeyDown(KeyCode.W)) player1.SetChoice(Choice.Rock);
        if (Input.GetKeyDown(KeyCode.A)) player1.SetChoice(Choice.Paper);
        if (Input.GetKeyDown(KeyCode.S)) player1.SetChoice(Choice.Scissors);

        // Player 2 input (Arrow keys) - this mirrors the Enemy update path but allows faster local input
        if (Input.GetKeyDown(KeyCode.UpArrow)) player2.SetChoice(Choice.Rock);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) player2.SetChoice(Choice.Paper);
        if (Input.GetKeyDown(KeyCode.DownArrow)) player2.SetChoice(Choice.Scissors);
    }

    void EvaluateWinner(Choice p1, Choice p2)
    {
        if (p1 == p2)
        {
            EndBattleRound("Draw! Again!");
            return;
        }

        bool p1Wins =
            (p1 == Choice.Rock && p2 == Choice.Scissors) ||
            (p1 == Choice.Paper && p2 == Choice.Rock) ||
            (p1 == Choice.Scissors && p2 == Choice.Paper);

        if (p1Wins)
        {
            player1Wins++;
            player2.ShowLosingSprite(p1); // player2 shows the sprite for the choice that beat them (p1)
            EndBattleRound("Player 1 Wins!");
        }
        else
        {
            player2Wins++;
            player1.ShowLosingSprite(p2);
            EndBattleRound("Player 2 Wins!");
        }
    }

    void EndBattleRound(string result)
    {
        resultText.text = result;

        if (player1Wins >= 2)
        {
            resultText.text = "Player 1 is the Champion!";
            bestOf3Button.gameObject.SetActive(false);
        }
        else if (player2Wins >= 2)
        {
            resultText.text = "Player 2 is the Champion!";
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
            player1Wins = 0;
            player2Wins = 0;
            roundCount = 0;
            StartCoroutine(StartBattleRound());
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}