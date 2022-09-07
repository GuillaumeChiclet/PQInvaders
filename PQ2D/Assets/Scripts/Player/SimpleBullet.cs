using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBullet : MonoBehaviour
{
    [SerializeField] private GameObject particlesOnHit;
    [SerializeField] private float lifeTime = 10.0f;
    private float lifeTimer = 0.0f;
    private void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer > lifeTime)
        {
            DestroyBullet();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.Damage();
        }

        DestroyBullet();
    }

    private void DestroyBullet()
    {
        // Here spawn FX & VFX
        if (particlesOnHit)
        {
            Instantiate(particlesOnHit, transform.position, particlesOnHit.transform.rotation);
        }

        Destroy(this.gameObject);
    }
}
