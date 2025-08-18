using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroStatView : MonoBehaviour
{
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private string modifiStat;

    [SerializeField] private float statMultiply;
    [SerializeField] private TMP_Text statMultiplyTxt;

    [SerializeField] private int cost;
    [SerializeField] private TMP_Text costTxt;

    private void OnEnable()
    {
        costTxt.text = cost.ToString();
        statMultiplyTxt.text = statMultiplyTxt.text.Replace("{n}", $"+{statMultiply}");
        upgradeBtn.onClick.AddListener(OnClickUpgradeBtn);
    }

    private void OnDisable()
    {
        upgradeBtn.onClick.RemoveListener(OnClickUpgradeBtn);
    }

    private void OnClickUpgradeBtn()
    {
        if (SaveSystem.PlayerData.money < cost)
        {
            upgradeBtn.transform.DOComplete();
            upgradeBtn.transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0.3f), 0.3f, 5, 0.5f);
            return;
        }

        AudioManager.Instance.PlaySound(AudioManager.Instance.audioClips.money, volume: 0.5f);
        var field = typeof(PlayerData).GetField(modifiStat);
        if (field == null) return;

        float currentValue = (float)field.GetValue(SaveSystem.PlayerData);
        var newValue = currentValue + statMultiply;
        field.SetValue(SaveSystem.PlayerData, newValue);

        SaveSystem.PlayerData.money -= cost;
        SaveSystem.PlayerData.Save();

        UIEnevts.OnUpdateMoney?.Invoke(SaveSystem.PlayerData.money);
    }
}