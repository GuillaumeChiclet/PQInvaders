using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    float timer = 0.0f;
    float waveDuration = 0.0f;
    float waveTimeBetweenSpawn = 0.0f;
    Period wavePeriod;

    bool waveRunning;


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

        waveTimeBetweenSpawn /= duration;
        waveDuration = duration;
    }


    private void Update()
    {
        if (timer < waveDuration)
        {
            timer += Time.deltaTime;
            return;
        }

        timer = 0.0f;

        float offset = Random.Range(0.0f, 1.0f);
        Vector3 position = Vector3.Lerp(spawnBornLeft.position, spawnBornRight.position, offset);
        Vector3 destination = Vector3.Lerp(targetBornLeft.position, targetBornRight.position, offset);

        switch (Random.Range(0, 1))
        {

        }
        Instantiate(enemyPrefabA, position, Quaternion.identity, this.transform);
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
