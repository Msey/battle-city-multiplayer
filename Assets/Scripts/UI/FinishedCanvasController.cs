using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using static GameConstants;

public class FinishedCanvasController : MonoBehaviour
{
    [SerializeField]
    private Text stageLabel;
    private int basicTanksCount;

    [SerializeField]
    private PlayerScoreColumn[] scoreColumns;


    protected void Awake()
    {
        Assert.IsNotNull(stageLabel);
        Assert.IsNotNull(scoreColumns);
        Assert.AreEqual(scoreColumns.Length, MAX_PLAYERS);
    }

    public PlayerScoreColumn ScoreColumn(int index)
    {
        return scoreColumns[index];
    }

    public void SetStage(int stage)
    {
        stageLabel.text = "STAGE " + stage.ToString();
    }

    public void SetPlayerCount(int count)
    {
        for (int playerIndex = 0; playerIndex < MAX_PLAYERS; ++playerIndex)
        {
            scoreColumns[playerIndex].gameObject.SetActive(playerIndex < count);
        }
    }
}
