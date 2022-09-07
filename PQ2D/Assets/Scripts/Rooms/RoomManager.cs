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


}
