using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

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

    private int gold = 0;
    private int roundCount = 0;
    private int playerWins = 0;
    private int enemyWins = 0;

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
            resultText.text = "You didn’t choose! You Lose!";
            enemyWins++;
            gold += 1; // losing reward
        }
        else
        {
            // Enemy randomly picks
            enemyChoice = (Choice)Random.Range(1, 4);
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

        // Rock: quick tap
        if (Input.GetMouseButtonDown(0))
        {
            playerChoice = Choice.Rock;
            resultText.text = "You chose Rock!";
        }

        // Paper: hold
        if (Input.GetMouseButton(0))
        {
            playerChoice = Choice.Paper;
            resultText.text = "You chose Paper!";
        }

        // Scissors: drag/swipe
        if (Input.GetMouseButton(0) && Mathf.Abs(Input.GetAxis("Mouse X")) > 0.2f)
        {
            playerChoice = Choice.Scissors;
            resultText.text = "You chose Scissors!";
        }
    }

    void EvaluateWinner()
    {
        if (playerChoice == enemyChoice)
        {
            // Draw case → immediately restart the countdown
            resultText.text = "Draw! Both chose " + playerChoice + ". Try again!";
            StartCoroutine(StartBattleRound());
            return;
        }

        // Player wins
        if ((playerChoice == Choice.Rock && enemyChoice == Choice.Scissors) ||
            (playerChoice == Choice.Paper && enemyChoice == Choice.Rock) ||
            (playerChoice == Choice.Scissors && enemyChoice == Choice.Paper))
        {
            resultText.text = "You Win! Enemy chose " + enemyChoice;
            playerWins++;
            gold += 3;
        }
        else // Player loses
        {
            resultText.text = "You Lose! Enemy chose " + enemyChoice;
            enemyWins++;
            gold += 1;
        }

        UpdateGoldText();
        roundCount++;

        // Now the round really ends
        bestOf3Button.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
    }

    // Button callbacks
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
            // Evaluate best of 3
            if (playerWins > enemyWins)
            {
                resultText.text = "You won the Best of 3! Bonus +5 Gold!";
                gold += 5;
            }
            else
            {
                resultText.text = "You lost the Best of 3. No bonus.";
            }

            UpdateGoldText();
        }
    }

    public void GoToMainMenu()
    {
        // Replace with scene loading when Main Menu is built
        Debug.Log("Return to main menu scene here");
    }

}