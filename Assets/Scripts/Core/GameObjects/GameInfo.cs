using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameInfo
{
    public enum EGameMode {
        Classic,
    }
    public EGameMode GameMode { get; set; } = EGameMode.Classic;
    public int CurrentStage { get; set; } = 0;
    public int PlayersCount { get; set; } = 0;

    public void Reset()
    {
        CurrentStage = 0;
        PlayersCount = 0;
    }
}
