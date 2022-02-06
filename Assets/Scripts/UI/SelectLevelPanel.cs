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
    private int _pageButtonsCount = 56;
    private int _currentPage = 0;
    private int pageCount
    {
        get
        {
            if (LevelsManager.s_InstanceExists)
            {
                return LevelsManager.s_Instance.LevelsCount / _pageButtonsCount;
            }
            else
            {
                return 0;
            }
        }
    }

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
        RegeneratePanel();
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void OnLeftButtonClicked()
    {
        if (_currentPage > 0)
        {
            --_currentPage;
        }
        RegeneratePanel();
    }

    public void OnRightButtonClicked()
    {
        if (_currentPage < (pageCount - 1))
        {
            ++_currentPage;
        }
        RegeneratePanel();
    }

    private void RegeneratePanel()
    {
        foreach (var child in _levelButtonsList)
        {
            Destroy(child.gameObject);
        }
        _levelButtonsList.Clear();

        for (int buttonIndex = 0; buttonIndex < _pageButtonsCount; ++buttonIndex)
        {
            int levelIndex = buttonIndex + _currentPage * _pageButtonsCount;
            if (levelIndex >= LevelsManager.s_Instance.LevelsCount)
                break;

            LevelButton currentButton = Instantiate(_levelButtonPrefab, _levelButtonsLayout).GetComponent<LevelButton>();
            currentButton.Level = levelIndex;
            currentButton.Locked = false;
            currentButton.OnSelect = OnLevelSelection;
            _levelButtonsList.Add(currentButton);
        }

        _leftButton.SetActive(_currentPage > 0);
        _rightButton.SetActive(_currentPage < (pageCount - 1));
    }

    private void OnLevelSelection(SelectionButtonTrigger trigger)
    {
        LevelsManager.s_Instance.CurrentGameInfo = new GameInfo();
        LevelsManager.s_Instance.CurrentGameInfo.PlayersCount = 1;
        LevelsManager.s_Instance.CurrentGameInfo.CurrentStage = trigger.Level;
        LevelsManager.s_Instance.StartGame();
    }
}
