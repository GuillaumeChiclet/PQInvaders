using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private int lifeMax = 3;
    private int life = 0;

    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;

        life = lifeMax;
    }

    public void Damage(int value = 1)
    {
        life -= value;
        life = Mathf.Clamp(life, 0, lifeMax);
        if (life == 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        agent.SetDestination(Vector3.zero);
    }


}
