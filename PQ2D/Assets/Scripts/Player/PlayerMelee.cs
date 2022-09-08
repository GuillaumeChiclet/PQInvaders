using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float range = 3.0f;
    [SerializeField] private float coneAngle = 30.0f;
    [SerializeField] private float timeBetween2Attacks = 0.5f;
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private Transform weapons;
    private float timer = 0.0f;

    private void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            return;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Attack();
            timer = timeBetween2Attacks;
        }
    }

    private void Attack()
    {
        weaponAnimator.SetTrigger("CAC");

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, weapons.right, range);

        foreach (RaycastHit2D hit in hits)
        {
            Vector3 dirToHit = (hit.point - (Vector2)transform.position);
            float angle = Vector3.Angle(weapons.right, dirToHit);
            if (angle <= coneAngle)
            {
                if (hit.collider.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    damageable.Damage(damage);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 left = Quaternion.Euler(0, 0, coneAngle) * weapons.right;
        Vector3 right = Quaternion.Euler(0, 0, -coneAngle) * weapons.right;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + left * range);
        Gizmos.DrawLine(transform.position, transform.position + right * range);

    }
}
