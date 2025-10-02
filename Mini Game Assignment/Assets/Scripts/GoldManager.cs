using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class GoldDisplay : MonoBehaviour
{
    public TMP_Text goldText;

    void OnEnable()
    {
        // Subscribe to sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateGoldText();
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateGoldText(); // always refresh when a new scene is loaded
    }

    public void UpdateGoldText()
    {
        int currentGold = PlayerPrefs.GetInt("Gold", 0);
        if (goldText != null)
            goldText.text = "Gold: " + currentGold;

    }
    
}