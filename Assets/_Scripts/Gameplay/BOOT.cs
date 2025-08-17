using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BOOT : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private GameObject logo;

    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private GroundMover groundMover;
    [SerializeField] private Player player;
    [SerializeField] private Transform startPos;

    private WaveManager waveManager;

    private void Awake()
    {
        waveManager = GetComponent<WaveManager>();
        waveManager.Initialize(gameConfig, player, groundMover);
    }

    private void OnEnable()
    {
        playBtn.onClick.AddListener(OnClickPlayBtn);
    }

    private void OnDisable()
    {
        playBtn.onClick.RemoveListener(OnClickPlayBtn);
    }

    private void OnClickPlayBtn()
    {
        playBtn.gameObject.SetActive(false);
        logo.SetActive(false);

        FlipPlayer();
        player.Move();

        float moveDuration = 1f;
        player.transform.DOMove(startPos.position, moveDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                FlipPlayer();
                player.Initialize();
                player.StopMove();
                StartCoroutine(waveManager.StartWave());
            });
    }

    private void FlipPlayer()
    {
        Vector3 scale = player.transform.localScale;
        scale.x *= -1;
        player.transform.localScale = scale;
    }
}