using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : Singleton<MainMenuManager>
{
    override protected void Awake()
    {
        base.Awake();
    }

    public void OnStartClassicGameClicked()
    {
        LevelsManager.s_Instance.OpenClassicGame();
    }
}
