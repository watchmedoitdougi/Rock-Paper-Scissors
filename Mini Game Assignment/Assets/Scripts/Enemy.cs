using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
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

        // Automatically detect if this scene is the 2-player version
        string sceneName = SceneManager.GetActiveScene().name;
        isTwoPlayerMode = sceneName.Contains("Player2Battle");
    }

    void Update()
    {
        if (isTwoPlayerMode)
        {
            // Player 2 uses arrow keys
            if (player2Choice != "None") return;

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                player2Choice = "Rock";
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                player2Choice = "Paper";
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                player2Choice = "Scissors";
        }
        else
        {
            // Single player mode: enemy picks randomly after countdown
            if (player2Choice == "None")
            {
                int random = Random.Range(1, 4); // 1-3
                switch (random)
                {
                    case 1: player2Choice = "Rock"; break;
                    case 2: player2Choice = "Paper"; break;
                    case 3: player2Choice = "Scissors"; break;
                }
            }
        }
    }

    public void ShowChoice(string choice)
    {
        switch (choice)
        {
            case "Rock": sr.sprite = rockSprite; break;
            case "Paper": sr.sprite = paperSprite; break;
            case "Scissors": sr.sprite = scissorsSprite; break;
            default: sr.sprite = defaultSprite; break;
        }
    }

    public void ResetSprite()
    {
        sr.sprite = defaultSprite;
        player2Choice = "None"; // Reset between rounds
    }
}