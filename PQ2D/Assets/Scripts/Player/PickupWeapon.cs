using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    [SerializeField] private int gunID = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerShoot>(out PlayerShoot playerShoot))
        {
            if (gunID == 1)
                playerShoot.PickupWeapon1();
            if (gunID == 2)
                playerShoot.PickupWeapon2();

            Destroy(this.gameObject);
        }
    }
}
