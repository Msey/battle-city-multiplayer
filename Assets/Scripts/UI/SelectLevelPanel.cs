using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

public class SelectLevelPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftButton;
    [SerializeField]
    private GameObject _rightButton;
    [SerializeField]
    private Transform _levelButtonsLayout;
    [SerializeField]
    private GameObject _levelButtonPrefab;

    private List<LevelButton> _levelButtonsList;

    void Awake()
    {
        Assert.IsNotNull(_leftButton);
        Assert.IsNotNull(_rightButton);
        Assert.IsNotNull(_levelButtonsLayout);
        Assert.IsNotNull(_levelButtonPrefab);

        _levelButtonsList = new List<LevelButton>();
    }

    public void Open()
    {
        GenerateLevelButtons();
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void GenerateLevelButtons()
    {
        foreach (var child in _levelButtonsList)
        {
            Destroy(child.gameObject);
        }
        _levelButtonsList.Clear();

        for (int i = 0; i < 10; ++i)
        {
            LevelButton currentButton = Instantiate(_levelButtonPrefab, _levelButtonsLayout).GetComponent<LevelButton>();
        }
    }
}
