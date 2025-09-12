using UnityEngine;
using UnityEngine.UI;

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

    private Vector3 mouseDownPos;
    private float mouseDownTime;
    private bool isHolding = false;

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
        Vector3 delta  = Input.mousePosition - mouseDownPos;

        if (Mathf.Abs(delta.x) > swipeThreshold && delta.x < 0.5f)
            return Choice.Scissors;

        if (isHolding)
            return Choice.Paper;

        if (heldTime <= clickTimeThreshold)
            return Choice.Rock;

        return Choice.Rock;
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