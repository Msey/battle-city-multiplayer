using System;
using InControl;
using UnityEngine;
using static GameConstants;

public class InputPlayer
{
    public InputPlayerActions PlayerActionSet { get; set; }
    public bool EnabledController { get; set; }
    public ITank Tank;

    private string saveData;
    private Direction lastDirection;

    void SaveBindings()
    {
        saveData = PlayerActionSet.Save();
        PlayerPrefs.SetString("Bindings", saveData);
    }

    void LoadBindings()
    {
        if (PlayerPrefs.HasKey("Bindings"))
        {
            saveData = PlayerPrefs.GetString("Bindings");
            PlayerActionSet.Load(saveData);
        }
    }

    public void Update()
    {
        if (PlayerActionSet.Fire.WasPressed || PlayerActionSet.FireA.IsPressed)
            Tank.Shoot();

        bool stopTank;

        Tank.Direction =
            lastDirection =
            GetStandartDirection(out stopTank);

        Tank.Stopped = stopTank;
    }

    public Direction GetStandartDirection(out bool stopTank)
    {
        stopTank = false;

        if (PlayerActionSet.Left.IsPressed)
            return Direction.Left;
        if (PlayerActionSet.Right.IsPressed)
            return Direction.Right;
        if (PlayerActionSet.Up.IsPressed)
            return Direction.Up;
        if (PlayerActionSet.Down.IsPressed)
            return Direction.Down;

        stopTank = true;

        return lastDirection;
    }
}
