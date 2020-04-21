using System;
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
    private int enemyTanksCount = 0;

    public LayoutGroup livesPanelLayout;
    public GameObject livesPanelPrefab;
    private List<GameObject> livesPanels;

    protected void Awake()
    {
        Assert.IsNotNull(currentStageText);

        tanksIcons = new List<GameObject>();
        Assert.IsNotNull(tanksCountLayout);
        Assert.IsNotNull(tanksPrefab);

        livesPanels = new List<GameObject>();
        Assert.IsNotNull(livesPanelLayout);
        Assert.IsNotNull(livesPanelPrefab);

        ClassicGameManager.GameStarted += OnGameStarted;
        EnemyTank.TankCreated += OnEnemyTankCreated;
    }

    private void OnDestroy()
    {
        EnemyTank.TankCreated -= OnEnemyTankCreated;
    }

    public void OnBackToMenuClicked()
    {
        LevelsManager.s_Instance.OpenMainMenu();
    }

    public void OnGameStarted(object sender, EventArgs e)
    {
        SetStage(LevelsManager.s_Instance.CurrentGameInfo.CurrentStage);
        LoadGameInfo();
    }

    private void LoadGameInfo()
    {
        if (ClassicGameManager.s_Instance != null)
            SetEnemyTanksCount(ClassicGameManager.s_Instance.LevelEnemeyTanksCount);
        else
            SetEnemyTanksCount(0);

        SetLivesPanelCount(LevelsManager.s_Instance.CurrentGameInfo.PlayersCount);
    }

    void OnEnemyTankCreated(object sender, EventArgs e)
    {
        SetEnemyTanksCount(enemyTanksCount - 1);
    }

    public void SetStage(int stage)
    {
        currentStageText.text = stage.ToString();
    }

    public void SetEnemyTanksCount(int count)
    {
        if (!Utils.Verify(count >= 0))
            return;

        enemyTanksCount = count;

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

    public void SetLivesPanelCount(int count)
    {
        if (!Utils.Verify(count >= 0))
            return;

        while (livesPanels.Count < count)
        {
            GameObject livesPanelObject = Instantiate(livesPanelPrefab);
            livesPanels.Add(livesPanelObject);
            livesPanelObject.transform.SetParent(livesPanelLayout.transform, false);

            var livesPanel = livesPanelObject.GetComponent<LivesPanel>();
            if (!Utils.Verify(livesPanel))
                continue;
            livesPanel.SetPlayerNumber(livesPanels.Count);
        }
        while (livesPanels.Count > count)
        {
            GameObject livesPanel = livesPanels[0];
            Destroy(livesPanel);
            livesPanels.RemoveAt(0);
        }
    }

    public void SetPlayerLives(int player, int lives)
    {
        if (!Utils.InRange(0, player, livesPanels.Count))
            return;

        GameObject livesPanelObject = livesPanels[player];
        if (!Utils.Verify(livesPanelObject))
            return;

        var livesPanel = livesPanelObject.GetComponent<LivesPanel>();
        if (!Utils.Verify(livesPanel))
            return;

        livesPanel.SetLives(lives);
    }
}
