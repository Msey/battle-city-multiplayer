using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetControlsMenuManager : MonoBehaviour
{
    public Transform[] PlayerControlButtonSet;

    private TankPlayerManager PlayerManager;

    private void Start()
    {
        PlayerManager = GameObject.Find("PlayerManager")
            .GetComponent<TankPlayerManager>();

        DefineKeyCodes();
    }

    private void DefineKeyCodes()
    {
        for (int i = 0; i < PlayerControlButtonSet.Length; i++)
        {
            foreach (Transform child in PlayerControlButtonSet[i])
            {
                string keyCodeString = child.GetChild(0).name.Split('_')[0];
                switch (keyCodeString)
                {
                    case "Left": child.GetChild(0).GetComponent<Text>().text = keyCodeString + ": " + PlayerManager.GetPlayerActionCode(i, TankPlayerManager.ActionType.Left); break;
                    case "Right": child.GetChild(0).GetComponent<Text>().text = keyCodeString + ": " + PlayerManager.GetPlayerActionCode(i, TankPlayerManager.ActionType.Right); break;
                    case "Up": child.GetChild(0).GetComponent<Text>().text = keyCodeString + ": " + PlayerManager.GetPlayerActionCode(i, TankPlayerManager.ActionType.Up); break;
                    case "Down": child.GetChild(0).GetComponent<Text>().text = keyCodeString + ": " + PlayerManager.GetPlayerActionCode(i, TankPlayerManager.ActionType.Down); break;
                    case "Fire": child.GetChild(0).GetComponent<Text>().text = keyCodeString + ": " + PlayerManager.GetPlayerActionCode(i, TankPlayerManager.ActionType.Fire); break;
                }
            }
        }
    }

    public void p1_OnFireAKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(1, TankPlayerManager.ActionType.FireA);
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


    public void p2_OnFireAKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(2, TankPlayerManager.ActionType.FireA);
    }

    public void p2_OnFireKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(2, TankPlayerManager.ActionType.Fire);
    }

    public void p2_OnLeftKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(2, TankPlayerManager.ActionType.Left);
    }

    public void p2_OnRightKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(2, TankPlayerManager.ActionType.Right);
    }

    public void p2_OnUpKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(2, TankPlayerManager.ActionType.Up);
    }

    public void p2_OnDownKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(2, TankPlayerManager.ActionType.Down);
    }


    public void p3_OnFireAKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(3, TankPlayerManager.ActionType.FireA);
    }

    public void p3_OnFireKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(3, TankPlayerManager.ActionType.Fire);
    }

    public void p3_OnLeftKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(3, TankPlayerManager.ActionType.Left);
    }

    public void p3_OnRightKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(3, TankPlayerManager.ActionType.Right);
    }

    public void p3_OnUpKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(3, TankPlayerManager.ActionType.Up);
    }

    public void p3_OnDownKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(3, TankPlayerManager.ActionType.Down);
    }


    public void p4_OnFireAKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(4, TankPlayerManager.ActionType.FireA);
    }

    public void p4_OnFireKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(4, TankPlayerManager.ActionType.Fire);
    }

    public void p4_OnLeftKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(4, TankPlayerManager.ActionType.Left);
    }

    public void p4_OnRightKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(4, TankPlayerManager.ActionType.Right);
    }

    public void p4_OnUpKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(4, TankPlayerManager.ActionType.Up);
    }

    public void p4_OnDownKey_BindingClick()
    {
        PlayerManager.BindPlayerKeyCode(4, TankPlayerManager.ActionType.Down);
    }


}
