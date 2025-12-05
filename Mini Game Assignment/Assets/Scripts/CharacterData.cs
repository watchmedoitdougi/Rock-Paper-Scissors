using UnityEngine;
[CreateAssetMenu(fileName = "CharacterData", menuName = "RPS/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite defaultSprite;
    public Sprite rockSprite;
    public Sprite paperSprite;
    public Sprite scissorsSprite;

    public Sprite loseToRockSprite;
    public Sprite loseToPaperSprite;
    public Sprite loseToScissorsSprite;
}
