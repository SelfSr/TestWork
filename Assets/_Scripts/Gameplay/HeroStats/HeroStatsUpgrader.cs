using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HeroStatsUpgrader : MonoBehaviour
{
    public event Action OnShowStats;

    [SerializeField] private Button closeBtn;

    private HeroStatView[] heroStats;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    public void Initialize()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        heroStats = GetComponentsInChildren<HeroStatView>(true);
        canvas = transform.root.GetComponent<Canvas>();

        closeBtn.onClick.AddListener(OnClickCloseBtn);
    }

    private void OnDestroy()
    {
        closeBtn.onClick.RemoveListener(OnClickCloseBtn);
    }

    public void ShowStats()
    {
        Show();
        float delay = 0f;

        for (int i = 0; i < heroStats.Length; i++)
        {
            var pos = heroStats[i].transform.position;
            heroStats[i].transform.position = new Vector3(heroStats[i].transform.position.x, heroStats[i].transform.position.y - 200f * canvas.scaleFactor, heroStats[i].transform.position.z);
            heroStats[i].transform.DOMove(pos, 1).SetDelay(delay);
            delay += 0.25f;
        }
    }

    private void Show()
    {
        canvasGroup.DOFade(1f, 0.5f);
        canvasGroup.blocksRaycasts = true;
    }

    private void Hide()
    {
        canvasGroup.DOFade(0f, 0.5f);
        canvasGroup.blocksRaycasts = false;
    }

    private void OnClickCloseBtn()
    {
        OnShowStats?.Invoke();
        Hide();
    }
}