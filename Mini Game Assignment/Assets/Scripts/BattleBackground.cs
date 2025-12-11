using UnityEngine;
using UnityEngine.UI;

public class BattleBackground : MonoBehaviour
{
    public Image backgroundImage;     // Your fullscreen UI Image
    public Sprite forestBG;
    public Sprite japanBG;
    public Sprite stageBG;

    public void SetBackground(string name)
    {
        switch (name)
        {
            case "Forest":
                backgroundImage.sprite = forestBG;
                break;
            case "Japan":
                backgroundImage.sprite = japanBG;
                break;
            case "Stage":
                backgroundImage.sprite = stageBG;
                break;
        }

        // This makes sure the sprite scales correctly
        backgroundImage.SetNativeSize();
    }
}
