using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private int lifeMax = 3;
    private int life = 0;

    NavMeshAgent agent;

    public UnityEvent OnDeath;

    Vector3 destination = Vector3.zero;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;

        life = lifeMax;
    }

    private void Start()
    {
        agent.SetDestination(Vector3.zero);
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
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

    public void Die()
    {
        OnDeath.Invoke();
        Destroy(gameObject);
    }

    


}
