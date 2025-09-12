using UnityEngine;
using UnityEngine.UI;
using static Player;

public class Player : MonoBehaviour
{
    public enum Choice { None, Rock, Paper, Scissors }

    [Header("Sprites")]
    private SpriteRenderer img;
    public Sprite rockSprite;
    public Sprite paperSprite;
    public Sprite scissorsSprite;
    public Sprite defaultSprite;

    [Header("Input Settings")]
    public float clickTimeThreshold = 0.2f;
    public float holdTimeThreshold  = 0.5f;
    public float swipeThreshold     = 50f;

    private Vector2 mouseDownPos;
    private float mouseDownTime;
    public bool isHolding = false;

    public Choice CurrentChoice { get; private set; } = Choice.None;


        void Start()
        {
            img = GetComponent<SpriteRenderer>();
            ResetToDefault();
        }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPos  = Input.mousePosition;
            mouseDownTime = Time.time;
            isHolding     = false;
        }

        if (Input.GetMouseButton(0))
        {
            if (Time.time - mouseDownTime > holdTimeThreshold)
                isHolding = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            CurrentChoice = DetectChoice();
            SetSprite(CurrentChoice);

            // Notify GameManager (guarded)
            if (GameManager.Instance != null)
                GameManager.Instance.PlayerHasChosen(CurrentChoice);
            else
                Debug.LogWarning("[Player] GameManager.Instance is null - can't notify round result.");
        }
    }

    Choice DetectChoice()
    {
        float heldTime = Time.time - mouseDownTime;

        // 1. Detect swipe first (any direction)
        Vector2 endTouch = Input.mousePosition;
        Vector2 swipeDelta = endTouch - mouseDownPos;

        if (swipeDelta.magnitude > 50f) // min swipe distance in pixels
        {
            return Choice.Scissors;
        }

        // 2. If no swipe, check if it was a hold
        if (heldTime > holdTimeThreshold)
        {
            return Choice.Paper;
        }

        // 3. If neither swipe nor hold, it’s a quick tap
        return Choice.Rock;
    }

    public void SetChoice(Player.Choice choice)
    {
        CurrentChoice = choice;
        SetSprite(choice);
    }

    // Public: set sprite according to a choice (handles None -> defaultSprite)
    public void SetSprite(Choice choice)
    {
        Sprite chosen = defaultSprite;
        switch (choice)
        {
            case Choice.Rock:     chosen = rockSprite;     break;
            case Choice.Paper:    chosen = paperSprite;    break;
            case Choice.Scissors: chosen = scissorsSprite; break;
            case Choice.None:
            default:
                chosen = defaultSprite; break;
        }

        if (img != null) img.sprite = chosen;
    }
     

    // Public: reset to default sprite and choice
    public void ResetToDefault()
    {
        CurrentChoice = Choice.None;
        SetSprite(Choice.None);
    }
}