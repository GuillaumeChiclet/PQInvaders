using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("Scriptable")]
    [SerializeField] private WeekScriptable week;

    [Header("Position")]
    [SerializeField] private Transform spawnBornLeft;
    [SerializeField] private Transform spawnBornRight;
    [SerializeField] private Transform targetBornLeft;
    [SerializeField] private Transform targetBornRight;

    [Header("Spawn")]
    [SerializeField] private GameObject enemyPrefabA;
    [SerializeField] private GameObject enemyPrefabB;

    private float timer = 0.0f;
    private float waveTimeBetweenSpawn = 0.0f;
    private Period wavePeriod = new Period();

    [HideInInspector] public UnityEvent OnAddEnemy = new();
    [HideInInspector] public UnityEvent OnRemoveEnemy = new();


    public List<EnemyController> enemies = new List<EnemyController>();
    public int enemyCounter = 0;
    float enemyUpdateTimer = 0.0f;
    float enemyUpdate = 0.2f;


    public void LaunchWave(int day, int period, float duration)
    {
        Period chosenPeriod = week.days[day].periods[period];

        wavePeriod = new Period();
        wavePeriod.enemiesNumber = new int[chosenPeriod.enemiesNumber.Length];
        
        for (int i = 0; i < chosenPeriod.enemiesNumber.Length; i++)
        {
            wavePeriod.enemiesNumber[i] = chosenPeriod.enemiesNumber[i];
            waveTimeBetweenSpawn += chosenPeriod.enemiesNumber[i];
        }

        waveTimeBetweenSpawn = duration / waveTimeBetweenSpawn;

        timer = waveTimeBetweenSpawn;
    }


    private void Update()
    {
        UpdateWave();

        if (enemyUpdateTimer < enemyUpdate)
        {
            enemyUpdateTimer += Time.deltaTime;
        }
        else
        {
            enemyUpdateTimer -= enemyUpdate;
            UpdateEnemies();
        }
            
    }

    private void UpdateWave()
    {
        if (timer < waveTimeBetweenSpawn)
        {
            timer += Time.deltaTime;
            return;
        }

        timer -= waveTimeBetweenSpawn;

        float offset = Random.Range(0.0f, 1.0f);
        Vector3 position = Vector3.Lerp(spawnBornLeft.position, spawnBornRight.position, offset);
        Vector3 destination = Vector3.Lerp(targetBornLeft.position, targetBornRight.position, offset);

        bool hasFirst = wavePeriod.enemiesNumber[0] > 0;
        bool hasSecond = wavePeriod.enemiesNumber[1] > 0;

        if (hasFirst && hasSecond)
        {
            int rng = Random.Range(0, 1);
            if (rng == 0)
            {
                SpawnEnemy(enemyPrefabA, 0, position, destination);
            }
            else
            {
                SpawnEnemy(enemyPrefabB, 1, position, destination);
            }
        }
        else
        {
            if (wavePeriod.enemiesNumber[0] > 0)
            {
                SpawnEnemy(enemyPrefabA, 0, position, destination);
            }
            else if (wavePeriod.enemiesNumber[1] > 0)
            {
                SpawnEnemy(enemyPrefabB, 1, position, destination);
            }
        }

        
    }

    private void UpdateEnemies()
    {
        if (enemyCounter >= enemies.Count)
            enemyCounter = 0;

        if (enemyCounter < enemies.Count)
            enemies[enemyCounter].UpdateDestination();

        enemyCounter++;
    }


    private void SpawnEnemy(GameObject prefab, int nb, Vector3 position, Vector3 destination)
    {
        wavePeriod.enemiesNumber[nb] -= 1;
        EnemyController enemy = Instantiate(prefab, position, Quaternion.identity, this.transform).GetComponent<EnemyController>();
        enemy.SetDestination(destination);
        enemy.OnDeath.AddListener(DeleteEnemy);
        enemies.Add(enemy);

        OnAddEnemy.Invoke();
    }

    private void DeleteEnemy(EnemyController enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);

            OnRemoveEnemy.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        if (spawnBornLeft && spawnBornRight && targetBornLeft && targetBornRight)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(spawnBornLeft.position, spawnBornRight.position);
            Gizmos.color = (Color.red + Color.yellow) * 0.5f;
            Gizmos.DrawLine(targetBornLeft.position, targetBornRight.position);
        }
    }

}
