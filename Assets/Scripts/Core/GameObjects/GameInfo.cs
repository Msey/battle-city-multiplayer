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
    public int CurrentStage { get; set; }
    public int PlayersCount { get; set; }
}
