using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [Header("Sprites (or assign CharacterData)")]
    public CharacterData data;
    public Sprite defaultSprite;
    public Sprite rockSprite;
    public Sprite paperSprite;
    public Sprite scissorsSprite;
    public Sprite loseToRockSprite;
    public Sprite loseToPaperSprite;
    public Sprite loseToScissorsSprite;

    [HideInInspector] public Choice choice = Choice.None;

    private SpriteRenderer sr;
    private bool isTwoPlayer = false;
    private bool inputEnabled = true;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null) Debug.LogError("Enemy requires a SpriteRenderer.");
        sr.sprite = GetDefaultSprite();

        string scene = SceneManager.GetActiveScene().name.ToLower();
        isTwoPlayer = scene.Contains("player2") || scene.Contains("2player");
    }

    void Update()
    {
        if (!inputEnabled) return;
        
        if (isTwoPlayer)
        { 
            if (choice != Choice.None) return;

            // Player 2 controls
            if (Input.GetKeyDown(KeyCode.LeftArrow)) choice = Choice.Rock;
            if (Input.GetKeyDown(KeyCode.RightArrow)) choice = Choice.Rock;
            if (Input.GetKeyDown(KeyCode.UpArrow)) choice = Choice.Paper;
            if (Input.GetKeyDown(KeyCode.DownArrow)) choice = Choice.Scissors;
        }
        else
        {
            // AI picks randomly when the round locks in
            // (Battle.cs will randomize if still None)
        }
    }

    Sprite GetDefaultSprite() => data != null ? data.defaultSprite : defaultSprite;
    Sprite GetRock() => data != null ? data.rockSprite : rockSprite;
    Sprite GetPaper() => data != null ? data.paperSprite : paperSprite;
    Sprite GetScissors() => data != null ? data.scissorsSprite : scissorsSprite;
    Sprite GetLoseToRock() => data != null ? data.loseToRockSprite : loseToRockSprite;
    Sprite GetLoseToPaper() => data != null ? data.loseToPaperSprite : loseToPaperSprite;
    Sprite GetLoseToScissors() => data != null ? data.loseToScissorsSprite : loseToScissorsSprite;

    public void SetChoice(Choice newChoice)
    {
        choice = newChoice;
        // Do NOT show the sprite yet — revealed later
    }

    public void RevealChoice()
    {
        switch (choice)
        {
            case Choice.Rock: sr.sprite = GetRock(); break;
            case Choice.Paper: sr.sprite = GetPaper(); break;
            case Choice.Scissors: sr.sprite = GetScissors(); break;
            default: sr.sprite = GetDefaultSprite(); break;
        }
    }


    public void ShowLosingSprite(Choice opponentChoice)
    {
        switch (opponentChoice)
        {
            case Choice.Rock: sr.sprite = GetLoseToRock(); break;
            case Choice.Paper: sr.sprite = GetLoseToPaper(); break;
            case Choice.Scissors: sr.sprite = GetLoseToScissors(); break;
            default: sr.sprite = GetDefaultSprite(); break;
        }
    }


    public void ResetSprite()
    {
        choice = Choice.None;
        sr.sprite = GetDefaultSprite();
    }

    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;
    }
}