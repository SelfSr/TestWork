using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Transform enemySpawnParent;

    private Player player;
    private GameConfig gameConfig;
    private GroundMover groundMover;
    private int currentWaveIndex;

    private List<Transform> spawnPos = new();
    private List<Enemy> enemies = new();

    private bool isWaveStarted;

    public void Initialize(GameConfig config, Player pl, GroundMover mover)
    {
        gameConfig = config;
        player = pl;
        groundMover = mover;

        player.OnEnemyDied += OnCharacterDied;

        for (int i = 0; i < enemySpawnParent.childCount; i++)
            spawnPos.Add(enemySpawnParent.GetChild(i));

        currentWaveIndex = 0;  // from savesystem
    }

    private void Update()
    {
        if (isWaveStarted && enemies != null && enemies.Count == 0)
        {
            isWaveStarted = false;
            currentWaveIndex = (currentWaveIndex + 1) % gameConfig.waves.Count;

            StartCoroutine(StartWave());
        }
    }

    public IEnumerator StartWave()
    {
        yield return new WaitForSeconds(1f);

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
                enemies.Remove(enemy);

            yield return new WaitForSeconds(1f);

            if (enemy != null)
                Destroy(enemy.gameObject);
        }
        else if (character is Player)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("Dead Player");
        }
    }

    private void OnDestroy()
    {
        if (enemies != null && enemies.Count != 0)
            foreach (var enemy in enemies)
                enemy.OnEnemyDied -= OnCharacterDied;

        player.OnEnemyDied -= OnCharacterDied;
    }
}