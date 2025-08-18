using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Globalization;

public class UIEnevts
{
    public static Action<int> OnUpdateMoney;
    public static Action<float, float, float> OnUpdateStats;
    public static Action OnShowLoseTxt;
}

public class UIManager : MonoBehaviour
{
    public Action OnClickPlayBtn;

    [SerializeField] private Button playBtn;
    [SerializeField] private Button statsBtn;
    [SerializeField] private GameObject logo;
    [SerializeField] private TMP_Text moneyTxt;
    [SerializeField] private TMP_Text statsInfo;
    [SerializeField] private TMP_Text loseTxt;

    private HeroStatsUpgrader stats;

    public void Initialize(HeroStatsUpgrader heroStats)
    {
        stats = heroStats;
        UpdateMoneyCounter(SaveSystem.PlayerData.money);
    }

    private void OnEnable()
    {
        playBtn.onClick.AddListener(ClickPlayBtn);
        statsBtn.onClick.AddListener(ShowStats);
        UIEnevts.OnUpdateMoney += UpdateMoneyCounter;
        UIEnevts.OnUpdateStats += UpdateStatsInfo;
        UIEnevts.OnShowLoseTxt += ShowLoseTxt;
    }

    private void OnDisable()
    {
        playBtn.onClick.RemoveListener(ClickPlayBtn);
        statsBtn.onClick.RemoveListener(ShowStats);
        UIEnevts.OnUpdateMoney -= UpdateMoneyCounter;
        UIEnevts.OnUpdateStats -= UpdateStatsInfo;
        UIEnevts.OnShowLoseTxt -= ShowLoseTxt;
    }

    private void ClickPlayBtn()
    {
        playBtn.gameObject.SetActive(false);
        statsBtn.gameObject.SetActive(false);
        logo.SetActive(false);
        moneyTxt.gameObject.SetActive(true);

        OnClickPlayBtn?.Invoke();
    }

    private void UpdateMoneyCounter(int value)
    {
        moneyTxt.text = value.ToString();

        moneyTxt.transform.DOComplete();
        moneyTxt.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f, 5, 0.5f);
    }

    private void UpdateStatsInfo(float damage, float health, float attackSpeed)
    {
        statsInfo.text = $"урон: {damage}\nжизни: {health}\nскорость атаки: {attackSpeed.ToString("F1", CultureInfo.InvariantCulture)}";
    }

    private void ShowLoseTxt()
    {
        loseTxt.DOFade(1f, 1f);
    }

    private void ShowStats()
    {
        moneyTxt.gameObject.SetActive(true);
        stats.ShowStats();
    }
}