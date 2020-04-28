using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetControlsMenuManager : MonoBehaviour
{


    public Transform[] PlayerControlButtonSets;

    private InputPlayerManager PlayerManager;

    private void Start()
    {
        PlayerManager = GameObject.Find("PlayerManager")
            .GetComponent<InputPlayerManager>();

        DefineKeyCodes();

        InputPlayerManager.OnBindingAdded += (s,e) =>
            {
                ButtonBeingClicked.transform.GetChild(0).GetComponent<Text>().color = Color.white;
                ButtonBeingClicked = null;
            };
    }

       
    private Button ButtonBeingClicked;

    const float TC = .5f;
    float timerColorChange = TC;


    private void ChangeColor()
    {
               
            if (ButtonBeingClicked.transform.GetChild(0).GetComponent<Text>().color == Color.red)
                ButtonBeingClicked.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            else
                ButtonBeingClicked.transform.GetChild(0).GetComponent<Text>().color = Color.red;
        
    }

    private void Update()
    {
        if (ButtonBeingClicked != null)
        {
            timerColorChange -= Time.deltaTime;

            if (timerColorChange < 0)
            {
                timerColorChange = TC;
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
                        {
                            components[setIndex].Add(keyCodeString, textComponent);
                            textComponent.text = keyCodeString + ": " + PlayerManager.GetPlayerActionCode(setIndex, InputPlayerManager.ActionType.Left);
                        }
                        break;
                    case "Right":
                        {
                            components[setIndex].Add(keyCodeString, textComponent);
                            textComponent.text = keyCodeString + ": " + PlayerManager.GetPlayerActionCode(setIndex, InputPlayerManager.ActionType.Right);
                        }
                        break;
                    case "Up":
                        {
                            components[setIndex].Add(keyCodeString, textComponent);
                            textComponent.text = keyCodeString + ": " + PlayerManager.GetPlayerActionCode(setIndex, InputPlayerManager.ActionType.Up);
                        }
                        break;
                    case "Down":
                        {
                            components[setIndex].Add(keyCodeString, textComponent);
                            textComponent.text = keyCodeString + ": " + PlayerManager.GetPlayerActionCode(setIndex, InputPlayerManager.ActionType.Down);
                        }
                        break;
                    case "Fire":
                        {
                            components[setIndex].Add(keyCodeString, textComponent);
                            textComponent.text = keyCodeString + ": " + PlayerManager.GetPlayerActionCode(setIndex, InputPlayerManager.ActionType.Fire);
                        }
                        break;
                    case "FireA":
                        {
                            components[setIndex].Add(keyCodeString, textComponent);
                            textComponent.text = keyCodeString + ": " + PlayerManager.GetPlayerActionCode(setIndex, InputPlayerManager.ActionType.FireA);
                        }
                        break;
                }

            }
        }

        PlayerManager.AssignButtonTextComponents(components);
    }


    public void p1_OnFireAKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(0, InputPlayerManager.ActionType.FireA);
    }

    public void p1_OnFireKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(0, InputPlayerManager.ActionType.Fire);
    }

    public void p1_OnLeftKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(0, InputPlayerManager.ActionType.Left);
    }

    public void p1_OnRightKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(0, InputPlayerManager.ActionType.Right);
    }

    public void p1_OnUpKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(0, InputPlayerManager.ActionType.Up);
    }

    public void p1_OnDownKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(0, InputPlayerManager.ActionType.Down);
    }

    public void p2_OnFireAKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(1, InputPlayerManager.ActionType.FireA);
    }

    public void p2_OnFireKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(1, InputPlayerManager.ActionType.Fire);
    }

    public void p2_OnLeftKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(1, InputPlayerManager.ActionType.Left);
    }

    public void p2_OnRightKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(1, InputPlayerManager.ActionType.Right);
    }

    public void p2_OnUpKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(1, InputPlayerManager.ActionType.Up);
    }

    public void p2_OnDownKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(1, InputPlayerManager.ActionType.Down);
    }

    public void p3_OnFireAKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(2, InputPlayerManager.ActionType.FireA);
    }

    public void p3_OnFireKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(2, InputPlayerManager.ActionType.Fire);
    }

    public void p3_OnLeftKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(2, InputPlayerManager.ActionType.Left);
    }

    public void p3_OnRightKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(2, InputPlayerManager.ActionType.Right);
    }

    public void p3_OnUpKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(2, InputPlayerManager.ActionType.Up);
    }

    public void p3_OnDownKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(2, InputPlayerManager.ActionType.Down);
    }


    public void p4_OnFireAKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(3, InputPlayerManager.ActionType.FireA);
    }

    public void p4_OnFireKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(3, InputPlayerManager.ActionType.Fire);
    }

    public void p4_OnLeftKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(3, InputPlayerManager.ActionType.Left);
    }

    public void p4_OnRightKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(3, InputPlayerManager.ActionType.Right);
    }

    public void p4_OnUpKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(3, InputPlayerManager.ActionType.Up);
    }

    public void p4_OnDownKey_BindingClick(Button sender)
    {
        ButtonBeingClicked = sender;
        PlayerManager.BindPlayerKeyCode(3, InputPlayerManager.ActionType.Down);
    }


}
