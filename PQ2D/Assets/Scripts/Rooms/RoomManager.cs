using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] EnemySpawner[] enemySpawners;

    public void LaunchWave(int day, int period, float duration)
    {
        for (int i = 0; i < enemySpawners.Length; i++)
        {
            enemySpawners[i].LaunchWave(day, period, duration);
        }
    }
}
