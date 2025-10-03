using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;


public class BattleManager : MonoBehaviour
{
    public TMP_Text rockText;
    public TMP_Text paperText;
    public TMP_Text scissorsText;
    public TMP_Text shootText;
    public TMP_Text resultText;

    public TMP_Text goldText;

    public Button bestOf3Button;
    public Button mainMenuButton;

    public PlayerBattle player;
    public EnemyBattle enemy;

    private enum Choice { None, Rock, Paper, Scissors }
    private Choice playerChoice = Choice.None;
    private Choice enemyChoice = Choice.None;

    private int roundCount = 0;
    private int playerWins = 0;
    private int enemyWins = 0;

    private Vector3 dragStart;
    private float holdTime;

    private bool inputLocked = false;

    void Start()
    {
        bestOf3Button.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
        StartCoroutine(StartBattleRound());
    }

    IEnumerator StartBattleRound()
    {
        inputLocked = false;
        playerChoice = Choice.None;
        enemyChoice = Choice.None;
        resultText.text = "";

        // Reset sprites to default at the start
        player.ResetSprite();
        enemy.ResetSprite();

        // Hide all countdown texts
        rockText.gameObject.SetActive(false);
        paperText.gameObject.SetActive(false);
        scissorsText.gameObject.SetActive(false);
        shootText.gameObject.SetActive(false);

        // Show Rock
        rockText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        rockText.gameObject.SetActive(false);

        // Show Paper
        paperText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        paperText.gameObject.SetActive(false);

        // Show Scissors
        scissorsText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        scissorsText.gameObject.SetActive(false);

        // Show Shoot!
        shootText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        shootText.gameObject.SetActive(false);

        // Give player 1 second to input
        yield return new WaitForSeconds(1);

        // Lock input
        inputLocked = true;

        // Enemy always picks after countdown
        enemyChoice = (Choice)Random.Range(1, 4);

        // Reveal both choices visually
        player.ShowChoice(playerChoice.ToString());
        enemy.ShowChoice(enemyChoice.ToString());

        // If player didn't pick
        if (playerChoice == Choice.None)
        {
            resultText.text = "Choose next time!";
            enemyWins++;
            AddGold(1); // losing reward
        }
        else
        {
            EvaluateWinner();
        }

        UpdateGoldText();

        roundCount++;

        // After each round, ask if they want Best of 3 or return
        bestOf3Button.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
    }

    void Update()
    {
        if (inputLocked) return;

        // Start tracking
        if (Input.GetMouseButtonDown(0))
        {
            dragStart = Input.mousePosition;
            holdTime = 0f;
        }

        // While holding, count time
        if (Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;
        }

        // Decide only when released
        if (Input.GetMouseButtonUp(0))
        {
            float dragDistance = Input.mousePosition.x - dragStart.x;

            if (Mathf.Abs(dragDistance) > 50f) // swipe threshold
            {
                playerChoice = Choice.Scissors;
                resultText.text = "You chose Scissors!";
            }
            else if (holdTime > 0.3f) // held long enough
            {
                playerChoice = Choice.Paper;
                resultText.text = "You chose Paper!";
            }
            else // quick tap
            {
                playerChoice = Choice.Rock;
                resultText.text = "You chose Rock!";
            }
        }


    }

    void EvaluateWinner()
    {
        if (playerChoice == enemyChoice)
        {
            // Draw → handled in EndBattleRound
            EndBattleRound("Draw! Again!");
            return;
        }

        // Player wins
        if ((playerChoice == Choice.Rock && enemyChoice == Choice.Scissors) ||
            (playerChoice == Choice.Paper && enemyChoice == Choice.Rock) ||
            (playerChoice == Choice.Scissors && enemyChoice == Choice.Paper))
        {
            EndBattleRound("You Win!");
        }
        else
        {
            EndBattleRound("You Lose!");
        }

        roundCount++;
    }

    void EndBattleRound(string result)
    {
        resultText.text = result;

        if (result.Contains("Win"))
        {
            AddGold(2);
            UpdateGoldText();

            // Won → only main menu
            mainMenuButton.gameObject.SetActive(true);
            bestOf3Button.gameObject.SetActive(false);
        }
        else if (result.Contains("Lose"))
        {
            // Lost → give redemption chance
            mainMenuButton.gameObject.SetActive(true);
            bestOf3Button.gameObject.SetActive(true);
        }
        else if (result.Contains("Draw"))
        {
            // Wait a moment before restarting the round
            StartCoroutine(RestartAfterDelay());
        }
    }

    IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(1.5f); // delay to show result
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
            if (playerWins > enemyWins)
            {
                resultText.text = "Nice! +5 Bonus !";
                AddGold(5);
            }
            else
            {
                resultText.text = "Loser! No Bonus";
            }

            UpdateGoldText();

            // After redemption, only main menu shows
            mainMenuButton.gameObject.SetActive(true);

            // Reset counters
            roundCount = 0;
            playerWins = 0;
            enemyWins = 0;
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void AddGold(int amount)
    {
        int currentGold = PlayerPrefs.GetInt("Gold", 0); // load existing or 0
        currentGold += amount;
        PlayerPrefs.SetInt("Gold", currentGold);
        PlayerPrefs.Save();
        UpdateGoldText();
    }

    void UpdateGoldText()
    {
        int currentGold = PlayerPrefs.GetInt("Gold", 0);
        if (goldText != null)
        {
            goldText.text = "Gold: " + currentGold;
        }
    }
}