using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine;

public class BOOT : MonoBehaviour
{
    [SerializeField] private UIManager uIManager;
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private GroundMover groundMover;
    [SerializeField] private Player player;
    [SerializeField] private Transform startPos;
    [SerializeField] private HeroStatsUpgrader heroStats;

    private WaveManager waveManager;

    private void Awake()
    {
        SaveSystem.Initialize();

        heroStats.Initialize();
        waveManager = GetComponent<WaveManager>();
        waveManager.Initialize(gameConfig, player, groundMover, heroStats);

        uIManager.Initialize(heroStats);
        uIManager.OnClickPlayBtn += StartGame;
    }

    private void OnDestroy()
    {
        uIManager.OnClickPlayBtn -= StartGame;
    }

    private void StartGame()
    {
        FlipPlayer();
        player.Move();

        float moveDuration = 1f;
        player.transform.DOMove(startPos.position, moveDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                FlipPlayer();
                player.StopMove();
                waveManager.IsGameStarted = true;
                waveManager.StartNewWave();
            });
    }

    private void FlipPlayer()
    {
        Vector3 scale = player.transform.localScale;
        scale.x *= -1;
        player.transform.localScale = scale;
    }
}