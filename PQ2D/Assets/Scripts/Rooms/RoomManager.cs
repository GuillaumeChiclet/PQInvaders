using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner[] enemySpawners;
    [SerializeField] private GameObject[] roomToActivate;
    [SerializeField] private GameObject[] wallToDeactivate;

    public bool activated = false;

    public void ActivateRoom()
    {
        activated = true;
        foreach (GameObject room in roomToActivate)
        {
            room.SetActive(true);
        }
        foreach (GameObject wall in wallToDeactivate)
        {
            wall.SetActive(false);
        }
    }

    

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
