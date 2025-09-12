using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    
    [Header("Sprites")]
    private SpriteRenderer img; // automatically grabs the Image component
    public Sprite rockSprite;
    public Sprite paperSprite;
    public Sprite scissorsSprite;
    public Sprite defaultSprite;

    public Player.Choice CurrentChoice { get; private set; } = Player.Choice.None;

    // Enemy picks randomly (1..3)
    public void PickRandomChoice()
    {
        CurrentChoice = (Player.Choice)Random.Range(1, 3); // 1..3
        SetSprite(CurrentChoice);
    }

    // Reset to default sprite & "None" choice
    public void ResetToDefault()
    {
        CurrentChoice = Player.Choice.None;
        SetSprite(CurrentChoice);
    }

    public void Start()
    {
        {
            img = GetComponent<SpriteRenderer>();
            ResetToDefault();
        }
    }

    // safe set sprite
    private void SetSprite(Player.Choice choice)
    {
        Sprite chosen = defaultSprite;
        switch (choice)
        {
            case Player.Choice.Rock: chosen = rockSprite; break;
            case Player.Choice.Paper: chosen = paperSprite; break;
            case Player.Choice.Scissors: chosen = scissorsSprite; break;
            case Player.Choice.None:
            default:
                chosen = defaultSprite; break;
        }

        if (img != null) img.sprite = chosen;
    }
      
}