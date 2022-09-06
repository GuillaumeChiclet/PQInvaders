using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Position")]
    [SerializeField] private Transform bornLeft;
    [SerializeField] private Transform bornRight;

    [Header("Spawn")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float timeBetweenSpawn = 2.0f;

    float timer = 0.0f;
    private void Update()
    {
        if (timer < timeBetweenSpawn)
        {
            timer += Time.deltaTime;
            return;
        }

        timer = 0.0f;

        Vector3 position = Vector3.Lerp(bornLeft.position, bornRight.position, Random.Range(0.0f, 1.0f));

        Instantiate(enemyPrefab, position, Quaternion.identity, this.transform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(bornLeft.position, bornRight.position);
    }

}
