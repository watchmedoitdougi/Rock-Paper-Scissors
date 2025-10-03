using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Multiplayer.Center.Common;


public class Battle2Player : MonoBehaviour
{
    public TMP_Text rockText;
    public TMP_Text paperText;
    public TMP_Text scissorsText;
    public TMP_Text shootText;
    public TMP_Text resultText;

    public Button bestOf3Button;
    public Button mainMenuButton;

    public Player1 player1;
    public Player2 player2;

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
        player1Choice = Choice.None;
        player2Choice = Choice.None;
        resultText.text = "";

        // Reset sprites to default at the start
        player1.ResetSprite();
        player2.ResetSprite();

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

        // Give players time to choose
        yield return new WaitForSeconds(1);

        // Lock input
        inputLocked = true;

        // Reveal both choices visually
        player1.ShowChoice(player1Choice.ToString());
        player2.ShowChoice(player2Choice.ToString());

        // If either player didn't pick
        if (player1Choice == Choice.None)
        {
            EndBattleRound("Nobody chose!");
        }
        else if (player1Choice == Choice.None)
        { 
            EndBattleRound("Player 2 Wins by default!");
            player2wins++;

         else if (player2Choice == Choice.None)
            {
                EndBattleRound("Player 1 Wins by default!");
                player1wins++;
            }
            else
            {
                EvaluateWinner();
            }

            roundCount++;

            // After Each round
            bestOf3Button.gameObject.SetActive(true);
            mainMenuButton.gameObject.SetActive(true);
        }
        

    }

    void Update()
    {
        if (!inputLocked)

        {
            // PLAYER 1 INPUT (WASD)
            if (player1Choice == Choice.None)
            {
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                    player1Choice = Choice.Rock;
                else if (Input.GetKeyDown(KeyCode.W))
                    player1Choice = Choice.Paper;
                else if (Input.GetKeyDown(KeyCode.S))
                    player1Choice = Choice.Scissors;

                // PLAYER 2 INPUT (Arrow Keys)
                if (player2choice == Choice.None && EnemyBattle.player2Choice != "None")
                {
                    player2Choice = (ISectionDependingOnUserChoices)System.Enum.Parse(typeof(Choice), enemy.player2Choice);
                }
            }
        }
      }

  
    void EvaluateWinner()
    {
        if (player1Choice == player2choice)
        {
           EndBattleRound("Draw! Again!");
            return;
        }

        // Player1 wins
        if ((player1Choice == Choice.Rock && player2Choice == Choice.Scissors) ||
            (player1Choice == Choice.Paper && player2choice == Choice.Rock) ||
            (player1Choice == Choice.Scissors && player2Choice == Choice.Paper))
        {
                player1Wins++;
                EndBattleRound("Player 1 Win!");
        }
        else
            {
                player2Wins++;
                EndBattleRound("Player 2 Wins!");

            roundCount++;
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
            bestOf3Button.gameObject.SetActive(true);
        }
        else if (result.Contains("Draw"))
        {
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
            // After redemption, only main menu shows
            mainMenuButton.gameObject.SetActive(true);

            // Reset counters
            roundCount = 0;
            player1Wins = 0;
            player2Wins = 0;
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}