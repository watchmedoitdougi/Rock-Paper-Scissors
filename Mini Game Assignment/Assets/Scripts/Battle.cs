using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Battle : MonoBehaviour
{

    //these are in the inspector until
    [Header("UI / Countdown")]
    public SpriteRenderer rockimage;
    public SpriteRenderer paperimage;
    public SpriteRenderer scissorsimage;
    public SpriteRenderer shootimage;
    public TMP_Text resultText;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject bestOf3Panel;

    [Header("Characters")]
    public PlayerBattle player;
    public Enemy enemy;

    public Image backgroundImage;
    public Sprite[] backgrounds;
    //here

    void Start()
    {
            SetRandomBackground();
            UpdateGoldText();
            StartCoroutine(StartBattleRound());
    }

    void SetRandomBackground()
    {
        int index = Random.Range(0, backgrounds.Length);
        backgroundImage.sprite = backgrounds[index];
    }



[Header("Gold")]
    public TMP_Text goldText;
    public int goldOnLose = 1;
    public int goldOnWin = 2;

    private bool inputLocked = false;


    IEnumerator StartBattleRound()
    {
        mainMenuPanel.SetActive(false);
        bestOf3Panel.SetActive(false);


        inputLocked = false;
        resultText.text = "";

        player.ResetSprite();
        enemy.ResetSprite();

        // Player can pick their choice BEFORE countdown
        player.SetInputEnabled(true);

        // Countdown (Rock → Paper → Scissors → Shoot!)
        yield return Countdown();

        // Lock input now that "Shoot!" has happened
        player.SetInputEnabled(false);

        // Grab final choices
        Choice playerChoice = player.choice;
        Choice enemyChoice = enemy.choice;

        // If enemy hasn't chosen, randomize
        if (enemyChoice == Choice.None)
            enemyChoice = (Choice)Random.Range(1, 4);

        // Reveal sprites
        player.RevealChoice();
        enemy.SetChoice(enemyChoice);

        // If player never made a choice, auto-lose
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

        // Start next round
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
