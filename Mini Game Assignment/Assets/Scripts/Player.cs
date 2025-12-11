using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    [Header("Sprites (or assign a CharacterData SO)")]
    public CharacterData data; // optional, can be null
    public Sprite defaultSprite;
    public Sprite rockSprite;
    public Sprite paperSprite;
    public Sprite scissorsSprite;
    public Sprite loseToRockSprite;
    public Sprite loseToPaperSprite;
    public Sprite loseToScissorsSprite;

    [HideInInspector] public Choice choice = Choice.None;

    private SpriteRenderer sr;

    // Input helpers
    private float mouseDownTime;
    private Vector2 mouseDownPos;
    private bool inputEnabled = true; // Battle controls rounds; disable if needed externally

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null) Debug.LogError("PlayerBattle requires a SpriteRenderer on the same GameObject.");
        sr.sprite = GetDefaultSprite();
    }

    void Update()
    {
        if (!inputEnabled) return; // Battle can disable if necessary

        // Click/hold/drag detection
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownTime = Time.time;
            mouseDownPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            float heldTime = Time.time - mouseDownTime;
            float dragDistance = Vector2.Distance(Input.mousePosition, mouseDownPos);

            // threshold values — tweak if needed
            if (dragDistance > 50f)
            {
                SetChoice(Choice.Scissors);
            }
            else if (heldTime > 0.35f)
            {
                SetChoice(Choice.Paper);
            }
            else
            {
                SetChoice(Choice.Rock);
            }
        }

        // Optional: keyboard fallback (WASD)
        if (Input.GetKeyDown(KeyCode.W)) SetChoice(Choice.Rock);
        if (Input.GetKeyDown(KeyCode.A)) SetChoice(Choice.Paper);
        if (Input.GetKeyDown(KeyCode.S)) SetChoice(Choice.Scissors);
    }

    // Helper getters that fall back to direct sprites if `data` is null
    Sprite GetDefaultSprite() => data != null ? data.defaultSprite : defaultSprite;
    Sprite GetRock() => data != null ? data.rockSprite : rockSprite;
    Sprite GetPaper() => data != null ? data.paperSprite : paperSprite;
    Sprite GetScissors() => data != null ? data.scissorsSprite : scissorsSprite;
    Sprite GetLoseToRock() => data != null ? data.loseToRockSprite : loseToRockSprite;
    Sprite GetLoseToPaper() => data != null ? data.loseToPaperSprite : loseToPaperSprite;
    Sprite GetLoseToScissors() => data != null ? data.loseToScissorsSprite : loseToScissorsSprite;

    // Called by Battle to set final visual or by input
    public void SetChoice(Choice newChoice)
    {
        choice = newChoice;
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


    // Shows the "I lost to X" sprite — argument is the opponent's winning choice
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

    // Optional: called by Battle to temporarily disable input while round resolves
    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;
    }
}
