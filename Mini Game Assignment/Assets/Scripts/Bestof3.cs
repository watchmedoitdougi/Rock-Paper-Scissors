using UnityEngine;
using System.Collections;

public class BestOf3Manager : MonoBehaviour
{
    [Header("Refs")]
    public Battle battle;            // assign your Battle object
    public GameObject mainMenuPanel; // reused main menu panel
    public GameObject bestOf3Button; // the single button shown after losing round1

    [Header("Timing")]
    public float delayAfterResult = 1.5f; // wait so player sees result before next round

    [HideInInspector] public bool IsActive = false;

    private int playerWins = 0;
    private int enemyWins = 0;

    void Start()
    {
        IsActive = false;
        // Hide best-of-3 button initially (Battle also hides it)
        if (bestOf3Button != null) bestOf3Button.SetActive(false);
    }

    // Called by the UI when player clicks the "Best of 3" button after losing round 1
    public void StartBestOf3AfterLoss()
    {
        // Setup scores: player already lost round1, so enemy has 1
        playerWins = 0;
        enemyWins = 1;
        IsActive = true;

        // Hide menu UI and start round 2 immediately
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (bestOf3Button != null) bestOf3Button.SetActive(false);

        battle.StartBattleRound();
    }

    // Called by Battle after each round. winner == "Player"/"Enemy"/"Draw"
    public void ReportResult(string winner)
    {
        if (!IsActive) return;

        if (winner == "Player") playerWins++;
        else if (winner == "Enemy") enemyWins++;

        // if draw: just proceed to next round after delay (no score change)
        if (winner == "Draw")
        {
            StartCoroutine(PlayNextRoundAfterDelay());
            return;
        }

        // someone reached 2? end match
        if (playerWins >= 2)
        {
            EndBo3(true);
            return;
        }
        if (enemyWins >= 2)
        {
            EndBo3(false);
            return;
        }

        // Otherwise continue automatically after a short delay
        StartCoroutine(PlayNextRoundAfterDelay());
    }

    IEnumerator PlayNextRoundAfterDelay()
    {
        // let player see resultText on screen
        yield return new WaitForSeconds(delayAfterResult);

        // Start next round (battle will reset sprites & run countdown)
        battle.StartBattleRound();
    }

    void EndBo3(bool playerWonMatch)
    {
        IsActive = false;

        // Show final message via Battle so same resultText is used
        if (playerWonMatch)
            battle.ShowFinalResult("You win best of 3!");
        else
            battle.ShowFinalResult("You lose best of 3!");

        // Show main menu so player can leave / restart
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }
}
