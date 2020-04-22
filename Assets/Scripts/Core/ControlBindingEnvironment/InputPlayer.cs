using System;
using InControl;
using UnityEngine;
using static GameConstants;

public class InputPlayer
{
    public InputPlayerActions PlayerActionSet { get; set; }
    public bool EnabledController { get; set; }

    private string saveData;

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

    public ITank Tank;

    public void Update()
    {
        if (PlayerActionSet.Fire.IsPressed || PlayerActionSet.FireA.IsPressed)
            Tank.Shoot();

        Tank.Stopped = false;
        Tank.Direction = this.GetStandartDirection(); // VITEK, fix kostilek
    }

    public Direction GetStandartDirection()
    {
        if (PlayerActionSet.Left.IsPressed)
            return Direction.Left;
        if (PlayerActionSet.Right.IsPressed)
            return Direction.Right;
        if (PlayerActionSet.Up.IsPressed)
            return Direction.Up;

        return Direction.Down;
    }
}
