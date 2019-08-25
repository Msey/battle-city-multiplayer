using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class HUDController : MonoBehaviour
{
    public Text currentStageText;
    public LayoutGroup tanksCountLayout;
    public GameObject tanksPrefab;
    private List<GameObject> tanksIcons;

    protected void Awake()
    {
        tanksIcons = new List<GameObject>();
        Assert.IsNotNull(currentStageText);
        Assert.IsNotNull(tanksCountLayout);
        Assert.IsNotNull(tanksPrefab);
    }

    private void Start()
    {
        EventManager.StartListening(GameEventBase.EventType.LevelStarted, OnGameStarted);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(GameEventBase.EventType.LevelStarted, OnGameStarted);
    }

    public void OnBackToMenuClicked()
    {
        LevelsManager.s_Instance.OpenMainMenu();
    }

    public void OnGameStarted(GameEventBase e)
    {
        SetStage(LevelsManager.s_Instance.CurrentGameInfo.CurrentStage);
        SetEnemyTanksCount(10);
    }

    public void SetStage(int stage)
    {
        currentStageText.text = stage.ToString();
    }

    public void SetEnemyTanksCount(int count)
    {
        if (!Utils.Verify(count >= 0))
            return;

        while (tanksIcons.Count < count)
        {
            GameObject tankIcon = Instantiate(tanksPrefab);
            tanksIcons.Add(tankIcon);
            tankIcon.transform.SetParent(tanksCountLayout.transform, false);
        }
        while (tanksIcons.Count > count)
        {
            GameObject tankIcon = tanksIcons[0];
            Destroy(tankIcon);
            tanksIcons.RemoveAt(0);
        }
    }
}
