using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetControlsMenuManager : MonoBehaviour
{
    string parentName = string.Empty;

    private TankPlayerManager PlayerManager;

    private void Start()
    {
        PlayerManager = GameObject.Find("PlayerManager")
            .GetComponent<TankPlayerManager>();        
    }


    public void p1_OnFireKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(1, TankPlayerManager.ActionType.Fire);
    }

    public void p1_OnLeftKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(1, TankPlayerManager.ActionType.Left);

    }

    public void p1_OnRightKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(1, TankPlayerManager.ActionType.Right);

    }

    public void p1_OnUpKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(1, TankPlayerManager.ActionType.Up);

    }

    public void p1_OnDownKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(1, TankPlayerManager.ActionType.Down);
    }    
}
