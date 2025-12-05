using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Battle : MonoBehaviour
{
    [Header("UI / Countdown")]
    public SpriteRenderer rockimage;
    public SpriteRenderer paperimage;
    public SpriteRenderer scissorsimage;
    public SpriteRenderer shootimage;
    public TMP_Text resultText;

    [Header("Characters")]
    public PlayerBattle player;
    public Enemy enemy;

    [Header("Gold")]
    public TMP_Text goldText;
    public int goldOnLose = 1;
    public int goldOnWin = 2;

    private bool inputLocked = false;

    void Start()
    {
        UpdateGoldText();
        StartCoroutine(StartBattleRound());
    }

    IEnumerator StartBattleRound()
    {
        inputLocked = false;
        resultText.text = "";

        player.ResetSprite();
        enemy.ResetSprite();

        yield return Countdown();

        // Lock input so players can't change choice while we reveal
        inputLocked = true;

        // Pull final choices (they should have been set by input in their own scripts)
        Choice playerChoice = player.choice;
        Choice enemyChoice = enemy.choice;

        // If enemy still has none (single-player), randomize it
        if (enemyChoice == Choice.None)
            enemyChoice = (Choice)Random.Range(1, 4);

        // Apply chosen sprites (calls on characters)
        player.SetChoice(playerChoice);
        enemy.SetChoice(enemyChoice);

        // Empty choice = auto loss
        if (playerChoice == Choice.None)
        {
            resultText.text = "No Choice — You Lose!";
            AddGold(goldOnLose);
        }
        else
        {
            EvaluateWinner(playerChoice, enemyChoice);
        }

        UpdateGoldText();

        yield return new WaitForSeconds(1.5f);

        // Allow next round
        StartCoroutine(StartBattleRound());
    }

    IEnumerator Countdown()
    {
        rockimage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        rockimage.gameObject.SetActive(false);

        paperimage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        paperimage.gameObject.SetActive(false);

        scissorsimage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        scissorsimage.gameObject.SetActive(false);

        shootimage.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        shootimage.gameObject.SetActive(false);
    }

    void EvaluateWinner(Choice playerChoice, Choice enemyChoice)
    {
        if (playerChoice == enemyChoice)
        {
            resultText.text = "Draw!";
            return;
        }

        bool playerWins =
            (playerChoice == Choice.Rock && enemyChoice == Choice.Scissors) ||
            (playerChoice == Choice.Paper && enemyChoice == Choice.Rock) ||
            (playerChoice == Choice.Scissors && enemyChoice == Choice.Paper);

        if (playerWins)
        {
            resultText.text = "You Win!";
            AddGold(goldOnWin);
            // enemy shows the losing sprite for *the choice that beat them* (playerChoice)
            enemy.ShowLosingSprite(playerChoice);
        }
        else
        {
            resultText.text = "You Lose!";
            AddGold(goldOnLose);
            // player shows losing sprite for the choice that beat them (enemyChoice)
            player.ShowLosingSprite(enemyChoice);
        }
    }

    void AddGold(int amount)
    {
        int current = PlayerPrefs.GetInt("Gold", 0);
        PlayerPrefs.SetInt("Gold", current + amount);
        PlayerPrefs.Save();
    }

    void UpdateGoldText()
    {
        if (goldText != null)
            goldText.text = "Gold: " + PlayerPrefs.GetInt("Gold", 0);
    }
}
