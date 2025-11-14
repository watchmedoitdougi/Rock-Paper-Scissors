using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "RPS/Character")]
public class CharacterData : ScriptableObject
{
    public string CharacterName;
    public Sprite idleSprite;
    public Sprite rockSprite;
    public Sprite paperSprite;
    public Sprite scissorsSprite;

    public Sprite loseRockSprite;
    public Sprite losePaperSprite;
    public Sprite loseScissorsSprite;
}
