using UnityEngine;
using UnityEngine.UI;

public class BattleBackground : MonoBehaviour
{
    public Image backgroundImage;  // The main background image

    public Sprite skateparkBG;
    public Sprite japanBG;
    public Sprite stageBG;

    public void SetBackground(string name)
    {
        switch (name)
        {
            case "Skatepark":
                backgroundImage.sprite = skateparkBG;
                break;
            case "Japan":
                backgroundImage.sprite = japanBG;
                break;
            case "Stage":
                backgroundImage.sprite = stageBG;
                break;
        }

    }

    public void SetRandomBackground()
    {
        int r = Random.Range(0, 3);

        switch (r)
        {
            case 0:
                SetBackground("Skatepark");
                break;
            case 1:
                SetBackground("Japan");
                break;
            case 2:
                SetBackground("Stage");
                break;
        }
    }
}
