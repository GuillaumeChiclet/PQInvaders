using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerShoot : MonoBehaviour
{
    public EventReference soundShoot;
    private FMOD.Studio.EventInstance instance;

    [Header("Shoot Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletImpulse;
    [SerializeField] private Vector2 bulletTorque;
    [SerializeField] private float timeBetween2bullets = 0.2f;
    [SerializeField] private int bulletAmountMax = 50;
    private int bulletAmount = 0;
    private float timerShoot = 0.0f;

    [Header("Spray Cone")]
    [SerializeField] private Vector2 coneAngleMinMax = new Vector2(15.0f, 25.0f);
    [SerializeField] private Vector2 sprayForce = new Vector2(0.2f, 0.5f);
    [SerializeField] private float sprayDeplete = 1.0f;

    [Header("Aim Sight")]
    [SerializeField] private float aimSightLength = 10.0f;
    [SerializeField] private Gradient aimSightColor;
    [SerializeField] private LineRenderer lineLeft;
    [SerializeField] private LineRenderer lineRight;

    float currentConeAngle = 15.0f;

    public bool IsShooting()
    {
        return timerShoot > 0.0f;
    }

    public bool IsFull()
    {
        return bulletAmount == bulletAmountMax;
    }

    public void Refill()
    {
        bulletAmount = bulletAmountMax;
    }

    private void Awake()
    {
        currentConeAngle = coneAngleMinMax.x;

        lineLeft.positionCount = 2;
        lineRight.positionCount = 2;
        lineLeft.colorGradient = aimSightColor;
        lineRight.colorGradient = aimSightColor;

        bulletAmount = bulletAmountMax;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetButton("Fire1"))
        {
            Shoot();
        }
        else
        {
            SprayDeplete();
        }

        UpdateConeLines();

        if (timerShoot > 0.0f)
        {
            timerShoot -= Time.deltaTime;
        }
    }

    private void Shoot()
    {
        if (IsShooting())
            return;

        timerShoot += timeBetween2bullets;

        if (bulletAmount <= 0)
        {
            // TODO : sound *clic clic* no more bullets
            return;
        }

        bulletAmount -= 1;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, Random.Range(-360, 360)));
        if (bullet.TryGetComponent<Rigidbody2D>(out Rigidbody2D bulletRb))
        {
            Vector3 shootDir = Quaternion.Euler(0, 0, Random.Range(-currentConeAngle, currentConeAngle)) * firePoint.right;
            bulletRb.AddForce(shootDir * bulletImpulse, ForceMode2D.Impulse);
            int rngRot = Random.Range(0, 2);
            if (rngRot == 0)
                bulletRb.angularVelocity = -Random.Range(bulletTorque.x, bulletTorque.y);
            else
                bulletRb.angularVelocity = Random.Range(bulletTorque.x, bulletTorque.y);
            Spray();
            instance = FMODUnity.RuntimeManager.CreateInstance(soundShoot);
            instance.start();
            instance.release();
        }
    }

    private void Spray()
    {
        currentConeAngle += Random.Range(sprayForce.x, sprayForce.y);
        currentConeAngle = Mathf.Clamp(currentConeAngle, coneAngleMinMax.x, coneAngleMinMax.y);
    }

    private void SprayDeplete()
    {
        currentConeAngle -= sprayDeplete * Time.deltaTime;
        currentConeAngle = Mathf.Clamp(currentConeAngle, coneAngleMinMax.x, coneAngleMinMax.y);
    }

    private void UpdateConeLines()
    {
        lineLeft.SetPosition(0, Vector3.zero);
        lineRight.SetPosition(0, Vector3.zero);

        Vector3 forwardLeft = Quaternion.Euler(0, 0, currentConeAngle) * Vector3.right;
        Vector3 forwardRight = Quaternion.Euler(0, 0, -currentConeAngle) * Vector3.right;

        lineLeft.SetPosition(1, forwardLeft * aimSightLength);
        lineRight.SetPosition(1, forwardRight * aimSightLength);
    }

}
