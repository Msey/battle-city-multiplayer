using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetControlsMenuManager : MonoBehaviour
{
    public Transform[] PlayerControlButtonSets;

    private const float BINDING_BUTTON_COLOR_CHANGE_TIME = 0.5f;

    private InputPlayerManager playerManager;
    private Button buttonBeingClicked;
    private float timerColorChange = BINDING_BUTTON_COLOR_CHANGE_TIME;

    private void Start()
    {
        playerManager = GameObject.Find("PlayerManager")
            .GetComponent<InputPlayerManager>();

        DefineKeyCodes();
        InputPlayerManager.OnKeyBindingAdded += FlushButtonBeingClicked;
    }

    private void ChangeColor()
    {
        var cachedText = buttonBeingClicked.transform.GetChild(0).GetComponent<Text>();

        if (cachedText.color == Color.red)
            cachedText.color = Color.white;
        else
            cachedText.color = Color.red;
    }

    private void Update()
    {
        if (buttonBeingClicked != null)
        {
            timerColorChange -= Time.deltaTime;

            if (timerColorChange < 0)
            {
                timerColorChange = BINDING_BUTTON_COLOR_CHANGE_TIME;
                ChangeColor();
            }
        }
    }

    private void DefineKeyCodes()
    {
        var components = new Dictionary<string, Text>[PlayerControlButtonSets.Length];

        for (int setIndex = 0; setIndex < PlayerControlButtonSets.Length; setIndex++)
        {
            components[setIndex] = new Dictionary<string, Text>();

            foreach (Transform child in PlayerControlButtonSets[setIndex])
            {
                string keyCodeString = child.GetChild(0).name.Split('_')[0];

                var textComponent = child.GetChild(0).GetComponent<Text>();
                switch (keyCodeString)
                {
                    case "Left":
                        components[setIndex].Add(keyCodeString, textComponent);
                        textComponent.text = keyCodeString + ": " + playerManager.GetPlayerActionCode(setIndex, InputPlayerManager.ActionType.Left);
                        break;
                    case "Right":
                        components[setIndex].Add(keyCodeString, textComponent);
                        textComponent.text = keyCodeString + ": " + playerManager.GetPlayerActionCode(setIndex, InputPlayerManager.ActionType.Right);
                        break;
                    case "Up":
                        components[setIndex].Add(keyCodeString, textComponent);
                        textComponent.text = keyCodeString + ": " + playerManager.GetPlayerActionCode(setIndex, InputPlayerManager.ActionType.Up);
                        break;
                    case "Down":
                        components[setIndex].Add(keyCodeString, textComponent);
                        textComponent.text = keyCodeString + ": " + playerManager.GetPlayerActionCode(setIndex, InputPlayerManager.ActionType.Down);
                        break;
                    case "Fire":
                        components[setIndex].Add(keyCodeString, textComponent);
                        textComponent.text = keyCodeString + ": " + playerManager.GetPlayerActionCode(setIndex, InputPlayerManager.ActionType.Fire);
                        break;
                    case "FireA":
                        components[setIndex].Add(keyCodeString, textComponent);
                        textComponent.text = keyCodeString + ": " + playerManager.GetPlayerActionCode(setIndex, InputPlayerManager.ActionType.FireA);
                        break;
                    case "Start":
                        components[setIndex].Add(keyCodeString, textComponent);
                        textComponent.text = keyCodeString + ": " + playerManager.GetPlayerActionCode(setIndex, InputPlayerManager.ActionType.Start);
                        break;
                }
            }
        }
        playerManager.AssignButtonTextComponents(components);
    }

    private void FlushButtonBeingClicked(object sender, System.EventArgs e)
    {
        buttonBeingClicked.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        buttonBeingClicked = null;
    }

    #region button_click_responses
    public void p1_OnFireAKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(0, InputPlayerManager.ActionType.FireA);
    }

    public void p1_OnFireKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(0, InputPlayerManager.ActionType.Fire);
    }

    public void p1_OnLeftKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(0, InputPlayerManager.ActionType.Left);
    }

    public void p1_OnRightKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(0, InputPlayerManager.ActionType.Right);
    }

    public void p1_OnUpKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(0, InputPlayerManager.ActionType.Up);
    }

    public void p1_OnDownKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(0, InputPlayerManager.ActionType.Down);
    }

    public void p1_OnStartKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(0, InputPlayerManager.ActionType.Start);
    }

    public void p2_OnFireAKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(1, InputPlayerManager.ActionType.FireA);
    }

    public void p2_OnFireKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(1, InputPlayerManager.ActionType.Fire);
    }

    public void p2_OnLeftKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(1, InputPlayerManager.ActionType.Left);
    }

    public void p2_OnRightKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(1, InputPlayerManager.ActionType.Right);
    }

    public void p2_OnUpKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(1, InputPlayerManager.ActionType.Up);
    }

    public void p2_OnDownKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(1, InputPlayerManager.ActionType.Down);
    }
    public void p2_OnStartKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(1, InputPlayerManager.ActionType.Start);
    }

    public void p3_OnFireAKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(2, InputPlayerManager.ActionType.FireA);
    }

    public void p3_OnFireKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(2, InputPlayerManager.ActionType.Fire);
    }

    public void p3_OnLeftKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(2, InputPlayerManager.ActionType.Left);
    }

    public void p3_OnRightKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(2, InputPlayerManager.ActionType.Right);
    }

    public void p3_OnUpKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(2, InputPlayerManager.ActionType.Up);
    }

    public void p3_OnDownKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(2, InputPlayerManager.ActionType.Down);
    }
    public void p3_OnStartKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(2, InputPlayerManager.ActionType.Start);
    }


    public void p4_OnFireAKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(3, InputPlayerManager.ActionType.FireA);
    }

    public void p4_OnFireKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(3, InputPlayerManager.ActionType.Fire);
    }

    public void p4_OnLeftKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(3, InputPlayerManager.ActionType.Left);
    }

    public void p4_OnRightKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(3, InputPlayerManager.ActionType.Right);
    }

    public void p4_OnUpKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(3, InputPlayerManager.ActionType.Up);
    }

    public void p4_OnDownKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(3, InputPlayerManager.ActionType.Down);
    }
    public void p4_OnStartKey_BindingClick(Button sender)
    {
        buttonBeingClicked = sender;
        playerManager.BindPlayerKeyCode(3, InputPlayerManager.ActionType.Start);
    }
    #endregion

}
