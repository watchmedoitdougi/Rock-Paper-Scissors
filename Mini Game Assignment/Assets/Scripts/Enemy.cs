using UnityEngine;
using UnityEngine.UI;

public class EnemyBattle : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite rockSprite;
    public Sprite paperSprite;
    public Sprite scissorsSprite;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = defaultSprite;
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