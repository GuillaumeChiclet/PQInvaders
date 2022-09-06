using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour
{
    [SerializeField] private float timeBeforeRespawn = 10.0f;
    [SerializeField] private GameObject ammunitionSprite;
    bool isOn = true;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isOn && collision.gameObject.TryGetComponent<PlayerShoot>(out PlayerShoot playerShoot))
        {
            playerShoot.Refill();
            
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        isOn = false;
        ammunitionSprite.SetActive(false);
        yield return new WaitForSeconds(timeBeforeRespawn);
        ammunitionSprite.SetActive(true);
        isOn = true;
    }
}
