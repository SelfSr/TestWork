using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Transform enemySpawnParent;

    private Player player;
    private GameConfig gameConfig;
    private GroundMover groundMover;
    private HeroStatsUpgrader stats;
    private int currentWaveIndex;

    private List<Transform> spawnPos = new();
    private List<Enemy> enemies = new();

    private bool isWaveStarted;
    public bool IsGameStarted { get; set; }

    public void Initialize(GameConfig config, Player pl, GroundMover mover, HeroStatsUpgrader heroStats)
    {
        gameConfig = config;
        player = pl;
        groundMover = mover;
        stats = heroStats;

        player.OnEnemyDied += OnCharacterDied;
        stats.OnShowStats += StartNewWave;

        for (int i = 0; i < enemySpawnParent.childCount; i++)
            spawnPos.Add(enemySpawnParent.GetChild(i));

        currentWaveIndex = SaveSystem.PlayerData.waveIndex;
    }

    private void OnDestroy()
    {
        if (enemies != null && enemies.Count != 0)
            foreach (var enemy in enemies)
                enemy.OnEnemyDied -= OnCharacterDied;

        player.OnEnemyDied -= OnCharacterDied;
        stats.OnShowStats -= StartNewWave;
    }

    private void Update()
    {
        if (isWaveStarted && enemies != null && enemies.Count == 0)
        {
            isWaveStarted = false;
            currentWaveIndex = (currentWaveIndex + 1) % gameConfig.waves.Count;
            SaveSystem.PlayerData.waveIndex = currentWaveIndex;
            SaveSystem.PlayerData.Save();

            stats.ShowStats();
        }
    }

    public void StartNewWave()
    {
        if (IsGameStarted)
            StartCoroutine(StartWave());
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(1f);
        player.Initialize();
        player.Move();
        groundMover.Move();

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < gameConfig.waves[currentWaveIndex].enemies.Count; i++)
        {
            Transform randSpawnPos = spawnPos[Random.Range(0, spawnPos.Count)];

            var enemy = Instantiate(gameConfig.waves[currentWaveIndex].enemies[i], randSpawnPos.position, Quaternion.identity, enemySpawnParent);

            enemy.Initialize();
            enemy.OnEnemyDied += OnCharacterDied;
            enemy.SetTarget(player.transform);
            enemies.Add(enemy);
        }

        yield return new WaitForSeconds(1f);

        player.SetTarget(enemies);
        player.StopMove();
        groundMover.Stop();

        isWaveStarted = true;
    }

    private void OnCharacterDied(Character character)
    {
        StartCoroutine(HandleCharacterDeathDelayed(character));
    }

    private IEnumerator HandleCharacterDeathDelayed(Character character)
    {
        if (character is Enemy enemy)
        {
            if (enemies.Contains(enemy))
            {
                enemies.Remove(enemy);
                UpdateMoney();
            }

            yield return new WaitForSeconds(1f);

            if (enemy != null)
                Destroy(enemy.gameObject);
        }
        else if (character is Player)
        {
            yield return new WaitForSeconds(1f);

            foreach (var ene in enemies)
            {
                ene.SetTarget(null);
            }

            UIEnevts.OnShowLoseTxt?.Invoke();

            yield return new WaitForSeconds(2f);

            SceneManager.LoadScene(0);
        }
    }

    private void UpdateMoney()
    {
        SaveSystem.PlayerData.money += 1;
        SaveSystem.PlayerData.Save();
        UIEnevts.OnUpdateMoney(SaveSystem.PlayerData.money);
    }
}