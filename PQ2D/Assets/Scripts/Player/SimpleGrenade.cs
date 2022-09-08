using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGrenade : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 3.0f;
    [SerializeField] private float repulsionForce = 20.0f;
    [SerializeField] private int damage = 2;
    [SerializeField] private GameObject particlesOnHit;
    [SerializeField] private float lifeTime = 4.0f;
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
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.up);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject == this.gameObject)
                continue;

            if (hit.rigidbody)
                hit.rigidbody.AddForce(-hit.normal * repulsionForce, ForceMode2D.Impulse);

            if (hit.collider.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.Damage(damage);
            }
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
