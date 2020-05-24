using static GameConstants;

public class GameInfo
{
    public enum EGameMode {
        Classic,
    }
    public EGameMode GameMode { get; set; }

    private int currentStage;
    public int CurrentStage
    {
        get => currentStage;
        set
        {
            if (value < 0)
                return;
            currentStage = value;
        }
    }
    public int PlayersCount { get; set; }
    public bool IsFirstGame { get; set; }
    public bool IsGameOver { get; set; }

    public int LivesCount { get; set; }
    public bool[] PrevLevelPlayerTankLiving = new bool[MAX_PLAYERS] { true, true, true, true };
    public PlayerTankStaticCharacteristicSet[] PrevLevelPlayerTankCharacteristic = new PlayerTankStaticCharacteristicSet[MAX_PLAYERS];

}
