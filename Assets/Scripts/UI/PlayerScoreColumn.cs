using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using static GameConstants;

public class PlayerScoreColumn : MonoBehaviour
{
    [SerializeField]
    private Text basicTanksCountText;
    private int basicTanksCount;

    [SerializeField]
    private Text fastTanksCountText;
    private int fastTanksCount;

    [SerializeField]
    private Text powerTanksCountText;
    private int powerTanksCount;

    [SerializeField]
    private Text armorTanksCountText;
    private int armorTanksCount;

    [SerializeField]
    private Text totalTanksCountText;

    protected void Awake()
    {
        Assert.IsNotNull(basicTanksCountText);
        Assert.IsNotNull(fastTanksCountText);
        Assert.IsNotNull(powerTanksCountText);
        Assert.IsNotNull(armorTanksCountText);
        Assert.IsNotNull(totalTanksCountText);
    }

    public void SetBasicTanksCount(int count)
    {
        basicTanksCount = count;
        basicTanksCountText.text = basicTanksCount.ToString();
        UpdateTotalTanksText();
    }

    public void SetFastTanksCount(int count)
    {
        fastTanksCount = count;
        fastTanksCountText.text = fastTanksCount.ToString();
        UpdateTotalTanksText();
    }

    public void SetPowerTanksCount(int count)
    {
        powerTanksCount = count;
        powerTanksCountText.text = powerTanksCount.ToString();
        UpdateTotalTanksText();
    }

    public void SetArmorTanksCount(int count)
    {
        armorTanksCount = count;
        armorTanksCountText.text = armorTanksCount.ToString();
        UpdateTotalTanksText();
    }

    public void SetTankCount(EnemyTankType tankType, int count)
    {
        switch (tankType)
        {
            case EnemyTankType.Fast:
                {
                    SetFastTanksCount(count);
                    break;
                }
            case EnemyTankType.Armor:
                {
                    SetArmorTanksCount(count);
                    break;
                }
            case EnemyTankType.Basic:
                {
                    SetBasicTanksCount(count);
                    break;
                }
            case EnemyTankType.Power:
                {
                    SetPowerTanksCount(count);
                    break;
                }
        }
    }

    private void UpdateTotalTanksText()
    {
        totalTanksCountText.text = (
            basicTanksCount + 
            fastTanksCount + 
            powerTanksCount +
            armorTanksCount).ToString();
    }
}
