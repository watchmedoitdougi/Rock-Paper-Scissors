using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Battle : MonoBehaviour
{
    [Header("Countdown UI")]
    public Image rockImage;
    public Image paperImage;
    public Image scissorsImage;
    public Image shootImage;
    public TMP_Text resultText;

    [Header("Characters")]
    public PlayerBattle player;
    public Enemy enemy;

    [Header("UI")]
    public GameObject mainMenuPanel;    // shows after normal win or after final Bo3
    public GameObject bestOf3Button;    // shows on normal loss to let player start Bo3

    [Header("Gold")]
    public TMP_Text goldText;
    public int goldOnLose = 1;
    public int goldOnWin = 2;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip drawSound;

    [Header("Manager")]
    public BestOf3Manager bestOf3Manager; // optional, assign if you use Bo3 manager

    private Coroutine runningRound;

    void Start()
    {
        UpdateGoldText();
        mainMenuPanel.SetActive(false);
        bestOf3Button.SetActive(false);

        StartBattleRound();

    }

    // Public entry used by UI or BestOf3Manager
    public void StartBattleRound()
    {
        // Cancel any existing round coroutine to be safe, then start new round.
        if (runningRound != null) StopCoroutine(runningRound);
        runningRound = StartCoroutine(BattleRoutine());
    }

    IEnumerator BattleRoutine()
    {
        // Reset UI & sprites
        mainMenuPanel.SetActive(false);
        bestOf3Button.SetActive(false);

        resultText.text = "";

        player.ResetSprite();
        enemy.ResetSprite();

        // Enable input
        player.SetInputEnabled(true);

        // Countdown
        yield return Countdown();

        // Lock input
        player.SetInputEnabled(false);

        // Get choices
        Choice pChoice = player.choice;
        Choice eChoice = enemy.choice;
        if (eChoice == Choice.None)
            eChoice = (Choice)Random.Range(1, 4);

        // Reveal visuals
        player.RevealChoice();
        enemy.SetChoice(eChoice);

        // Evaluate and react
        string winner = EvaluateRound(pChoice, eChoice);

        // Play win/lose sound BEFORE Bo3 exits
        if (winner == "Player")
            PlayResultSound(true);
        else if (winner == "Enemy")
            PlayResultSound(false);

        // If we're in a Bo3 flow managed externally, report result to the manager:
        if (bestOf3Manager != null && bestOf3Manager.IsActive)
        {
            bestOf3Manager.ReportResult(winner);
            yield break; // now safe, sound already played
        }

        // Normal (non-Bo3) behavior:
        if (winner == "Enemy")
        {
            // Player lost normal match -> allow them to start Bo3 or return to menu
            bestOf3Button.SetActive(true);
            mainMenuPanel.SetActive(true);
        }
        else if (winner == "Player")
        {
            // Player won -> go back to menu
            mainMenuPanel.SetActive(true);
        }
        else // Draw
        {
            // For a normal draw we can auto-restart a single new round after short delay
            yield return new WaitForSeconds(1.2f);
            StartBattleRound();
        }

        // Play win/lose sounds
        if (winner == "Player")
            PlayResultSound(true);
        else if (winner == "Enemy")
            PlayResultSound(false);
    }

    public void PlayResultSound(bool didWin)
    {
        if (audioSource == null) return;

        if (didWin && winSound != null)
            audioSource.PlayOneShot(winSound);
        else if (!didWin && loseSound != null)
            audioSource.PlayOneShot(loseSound);
    }


    IEnumerator Countdown()
    {
        rockImage.gameObject.SetActive(false);
        paperImage.gameObject.SetActive(false);
        scissorsImage.gameObject.SetActive(false);
        shootImage.gameObject.SetActive(false);

        rockImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        rockImage.gameObject.SetActive(false);

        paperImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        paperImage.gameObject.SetActive(false);

        scissorsImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        scissorsImage.gameObject.SetActive(false);

        shootImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        shootImage.gameObject.SetActive(false);
    }

    // Returns "Player", "Enemy" or "Draw"
    string EvaluateRound(Choice playerChoice, Choice enemyChoice)
    {
        if (playerChoice == Choice.None)
        {
            resultText.text = "No Choice — You Lose!";
            AddGold(goldOnLose);
            return "Enemy";
        }

        if (playerChoice == enemyChoice)
        {
            resultText.text = "Draw!";

            // Play draw sound
            if (audioSource != null && drawSound != null)
                audioSource.PlayOneShot(drawSound);

            return "Draw";
        }

        bool playerWins =
            (playerChoice == Choice.Rock && enemyChoice == Choice.Scissors) ||
            (playerChoice == Choice.Paper && enemyChoice == Choice.Rock) ||
            (playerChoice == Choice.Scissors && enemyChoice == Choice.Paper);

        if (playerWins)
        {
            resultText.text = "You Win!";
            AddGold(goldOnWin);
            enemy.ShowLosingSprite(playerChoice);
            return "Player";
        }
        else
        {
            resultText.text = "You Lose!";
            AddGold(goldOnLose);
            player.ShowLosingSprite(enemyChoice);
            return "Enemy";
        }
    }



    public void ShowFinalResult(string message)
    {
        // Called by BestOf3Manager at the end of Bo3
        resultText.text = message;
        mainMenuPanel.SetActive(true);
    }

    void AddGold(int amount)
    {
        int current = PlayerPrefs.GetInt("Gold", 0);
        PlayerPrefs.SetInt("Gold", current + amount);
        PlayerPrefs.Save();
        UpdateGoldText();
    }

    void UpdateGoldText()
    {
        if (goldText != null)
            goldText.text = "Gold: " + PlayerPrefs.GetInt("Gold", 0);
    }

    public void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
