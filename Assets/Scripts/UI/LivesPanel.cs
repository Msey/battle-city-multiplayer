using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class LivesPanel : MonoBehaviour
{
    public Text playerNumberText;
    public Text livesText;

    protected void Awake()
    {
        Assert.IsNotNull(playerNumberText);
        Assert.IsNotNull(playerNumberText);
    }

    public void SetPlayerNumber(int number)
    {
        playerNumberText.text = Utils.ToRoman(number) + "P";
    }

    public void SetLives(int lives)
    {
        livesText.text = lives.ToString();
    }
}
