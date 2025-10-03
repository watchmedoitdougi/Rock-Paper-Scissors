using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyBattle : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite rockSprite;
    public Sprite paperSprite;
    public Sprite scissorsSprite;

    private SpriteRenderer sr;

    [HideInInspector] public string player2Choice = "None";
    private bool isTwoPlayerMode = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = defaultSprite;

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Player2Battle")
        {
            isTwoPlayerMode = true;
        }
    }

    void Update()
    {
        // Only process input if this is the 2-player battle
        if (!isTwoPlayerMode) return;

        // Only take input once (BattleManager will check this value)
        if (player2Choice != "None") return;

        // Player 2 uses arrow keys
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            player2Choice = "Rock";
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            player2Choice = "Paper";
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            player2Choice = "Scissors";

        public void ShowChoice(string choice)
    {
        switch (choice)
        {
            case "Rock":
                sr.sprite = rockSprite;
                break;
            case "Paper":
                sr.sprite = paperSprite;
                break;
            case "Scissors":
                sr.sprite = scissorsSprite;
                break;
            default:
                sr.sprite = defaultSprite;
                break;
        }
    }

    public void ResetSprite()
    {
        sr.sprite = defaultSprite;
    }
}