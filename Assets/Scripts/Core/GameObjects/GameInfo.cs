using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
}
