using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class Battle2Player : MonoBehaviour
{
    [Header("Countdown UI")]
    public Image rockimage;
    public Image paperimage;
    public Image scissorsimage;
    public Image shootimage;

    [Header("UI")]
    public TMP_Text resultText;
    public Button bestOf3Button;
    public Button mainMenuButton;

    [Header("Players")]
    public PlayerBattle player1;   // WASD controls
    public Enemy player2;          // Arrow key controls

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip drawSound;

    [Header("Backgrounds")]
    public BattleBackground backgroundManager;

    private bool inputLocked = false;

    private int player1Wins = 0;
    private int player2Wins = 0;
    private int roundCount = 0;

    void Start()
    {
        bestOf3Button.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);

        if (backgroundManager != null)
            backgroundManager.SetRandomBackground();

        StartCoroutine(StartBattleRound());
    }

    IEnumerator StartBattleRound()
    {
        if (backgroundManager != null)
            backgroundManager.SetRandomBackground();

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

        // Input window
        yield return new WaitForSeconds(1);

        // Lock input
        inputLocked = true;

        // Reveal sprites
        player1.RevealChoice();
        player2.RevealChoice();

        Choice p1 = player1.choice;
        Choice p2 = player2.choice;

        // Handle no input
        if (p1 == Choice.None && p2 == Choice.None)
        {
            PlayDrawSound();
            EndBattleRound("Nobody chose!");
        }
        else if (p1 == Choice.None)
        {
            player2Wins++;
            PlayLoseSound();   // P1 loses
            player1.ShowLosingSprite(Choice.None);
            EndBattleRound("Player 2 Wins by default!");
        }
        else if (p2 == Choice.None)
        {
            player1Wins++;
            PlayWinSound();    // P1 wins
            player2.ShowLosingSprite(Choice.None);
            EndBattleRound("Player 1 Wins by default!");
        }
        else
        {
            EvaluateWinner(p1, p2);
        }

        roundCount++;
    }

    void Update()
    {
        if (inputLocked) return;

        // Player 1 (WASD)
        if (Input.GetKeyDown(KeyCode.W)) player1.SetChoice(Choice.Rock);
        if (Input.GetKeyDown(KeyCode.A)) player1.SetChoice(Choice.Paper);
        if (Input.GetKeyDown(KeyCode.S)) player1.SetChoice(Choice.Scissors);

        // Player 2 (arrow keys)
        if (Input.GetKeyDown(KeyCode.UpArrow)) player2.SetChoice(Choice.Rock);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) player2.SetChoice(Choice.Paper);
        if (Input.GetKeyDown(KeyCode.DownArrow)) player2.SetChoice(Choice.Scissors);
    }

    void EvaluateWinner(Choice p1, Choice p2)
    {
        if (p1 == p2)
        {
            PlayDrawSound();
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
            PlayWinSound();
            player2.ShowLosingSprite(p1);
            EndBattleRound("Player 1 Wins!");
        }
        else
        {
            player2Wins++;
            PlayLoseSound();
            player1.ShowLosingSprite(p2);
            EndBattleRound("Player 2 Wins!");
        }
    }

    void EndBattleRound(string result)
    {
        resultText.text = result;

        // Champion
        if (player1Wins >= 2)
        {
            resultText.text = "Player 1 is the Champion!";
            bestOf3Button.gameObject.SetActive(true);
            mainMenuButton.gameObject.SetActive(true);
            return;
        }
        if (player2Wins >= 2)
        {
            resultText.text = "Player 2 is the Champion!";
            bestOf3Button.gameObject.SetActive(true);
            mainMenuButton.gameObject.SetActive(true);
            return;
        }

        // Continue match
        StartCoroutine(RestartAfterDelay());
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

    void PlayWinSound()
    {
        if (audioSource != null && winSound != null)
            audioSource.PlayOneShot(winSound);
    }

    void PlayLoseSound()
    {
        if (audioSource != null && loseSound != null)
            audioSource.PlayOneShot(loseSound);
    }

    void PlayDrawSound()
    {
        if (audioSource != null && drawSound != null)
            audioSource.PlayOneShot(drawSound);
    }
}
