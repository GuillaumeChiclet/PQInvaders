using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] spriteShuffle;
    [SerializeField] private GameObject particlesOnHit;
    [SerializeField] private GameObject particlesOnDeath;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private int lifeMax = 3;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float timeHit = 0.3f;
    private float timerHit = 0.0f;
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

        //spriteRenderer.sprite = spriteShuffle[Random.Range(0, spriteShuffle.Length)];
    }

    private void Update()
    {
        if (timerHit > 0.0f)
        {
            timerHit -= Time.deltaTime;
        }
        else
        {
            spriteRenderer.color = Color.white;
            timerHit = 0.0f;
        }

        if (stopped)
            return;

        spriteRenderer.flipX = agent.desiredVelocity.x > 0;
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
        if (particlesOnHit)
        {
            Instantiate(particlesOnHit, transform.position, particlesOnHit.transform.rotation);
        }

        timerHit = timeHit;
        spriteRenderer.color = Color.red;

        if (life == 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (particlesOnDeath)
        {
            Instantiate(particlesOnDeath, transform.position, particlesOnDeath.transform.rotation);
        }
        OnDeath.Invoke(this);
        Destroy(gameObject, timeHit);
    }

    public void Stop()
    {
        stopped = true;
    }
    
    
}
