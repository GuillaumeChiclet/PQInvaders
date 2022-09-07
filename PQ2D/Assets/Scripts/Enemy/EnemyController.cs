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

    private NavMeshAgent agent;
    private Rigidbody2D rb;

    private Vector3 destination = Vector3.zero;
    private bool stopped = false;

    public UnityEvent<EnemyController> OnDeath = new();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;

        life = lifeMax;
    }

    private void Update()
    {
        if (stopped)
            return;

        transform.right = Vector3.MoveTowards(transform.right, agent.desiredVelocity.normalized, 50.0f * Time.deltaTime);
    }

    public void UpdateDestination()
    {
        if (stopped)
            return;

        agent.SetDestination(destination);
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        agent.SetDestination(destination);
    }

    public void Damage(int value = 1)
    {
        life -= value;
        life = Mathf.Clamp(life, 0, lifeMax);
        if (life == 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDeath.Invoke(this);
        Destroy(gameObject);
    }

    public void Stop()
    {
        stopped = true;
    }
    
    
}
