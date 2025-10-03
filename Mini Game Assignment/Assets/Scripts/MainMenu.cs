using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Called when you click the "Battle" button
    public void StartBattle()
    {
        SceneManager.LoadScene("BattleScene");
    }

    // (Optional) Called when you click the "Shop" button
    public void OpenShop()
    {
        SceneManager.LoadScene("ShopScene");
    }

    public void Start2player()
    {
        SceneManager.LoadScene("2PlayerScene");
    }
}