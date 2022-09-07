using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner[] enemySpawners;

    public void SetupSpawners(UnityAction add, UnityAction remove)
    {
        foreach (EnemySpawner spawner in enemySpawners)
        {
            spawner.OnAddEnemy.AddListener(add);
            spawner.OnRemoveEnemy.AddListener(remove);
        }
    }

    public void LaunchWave(int day, int period, float duration)
    {
        for (int i = 0; i < enemySpawners.Length; i++)
        {
            enemySpawners[i].LaunchWave(day, period, duration);
        }
    }
}
