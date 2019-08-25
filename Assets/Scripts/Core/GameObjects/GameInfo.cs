using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameInfo
{
    public enum EGameMode {
        classic,
    }
    public EGameMode GameMode { get; set; } = EGameMode.classic;
    public int CurrentStage { get; set; } = 0;
    public int PlayerCount { get; set; } = 0;

    public void Reset()
    {
        CurrentStage = 0;
        PlayerCount = 0;
    }
}
