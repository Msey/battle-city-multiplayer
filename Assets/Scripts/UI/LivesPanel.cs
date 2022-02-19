using UnityEngine;
using TMPro;
using UnityEngine.Assertions;

public class LivesPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI livesText;

    protected void Awake()
    {
        Assert.IsNotNull(livesText);
    }

    public void SetLives(int lives)
    {
        livesText.text = lives.ToString();
    }
}
