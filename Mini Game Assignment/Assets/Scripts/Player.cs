using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerBattle : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite rockSprite;
    public Sprite paperSprite;
    public Sprite scissorsSprite;

    private SpriteRenderer sr;
    private bool isTwoPlayerMode = false;

    [HideInInspector] public string player1Choice = "None";

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = defaultSprite;

        string sceneName = SceneManager.GetActiveScene().name;
        isTwoPlayerMode = sceneName.Contains("Player2Battle");
    }

    void Update()
    {
        if (!isTwoPlayerMode) return; // only 2-player mode uses keyboard

        if (player1Choice != "None") return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            player1Choice = "Rock";
        else if (Input.GetKeyDown(KeyCode.W))
            player1Choice = "Paper";
        else if (Input.GetKeyDown(KeyCode.S))
            player1Choice = "Scissors";
    }

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