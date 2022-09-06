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
    float timer = 0.0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;

        life = lifeMax;
    }

    private void Update()
    {
        if (timer < 3.0f)
        {
            timer += Time.deltaTime;
            return;
        }

        agent.SetDestination(destination);
        timer = 0.0f;
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
            Destroy(this.gameObject);
        }
    }

    public void Die()
    {
        OnDeath.Invoke();
        Destroy(gameObject);
    }

    


}
