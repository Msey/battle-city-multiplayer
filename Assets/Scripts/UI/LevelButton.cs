using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Assertions;

public class SelectionButtonTrigger : EventArgs
{
    public readonly int Level;
    public SelectionButtonTrigger(int level)
    {
        Level = level;
    }
}

public class LevelButton : MonoBehaviour
{
    private int _level;
    public int Level
    {
        get
        {
            return _level;
        }
        set
        {
            _level = value;
            _levelText.text = (value + 1).ToString();
        }
    }

    public bool Locked
    {
        get
        {
            return !GetComponent<Button>().interactable;
        }
        set
        {
            GetComponent<Button>().interactable = !value;
        }
    }

    public delegate void LevelSelected(SelectionButtonTrigger trigger);
    public LevelSelected OnSelect;

    [SerializeField]
    private TMPro.TextMeshProUGUI _levelText;

    public void OnClicked()
    {
        OnSelect(new SelectionButtonTrigger(_level));
    }

    protected void Awake()
    {
        Assert.IsNotNull(_levelText);
    }
}
