using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerShoot : MonoBehaviour
{
    public EventReference soundShoot;
    private FMOD.Studio.EventInstance instance;
    public EventReference soudnUnshoot;
    private FMOD.Studio.EventInstance instance2;
    public EventReference soundReload;
    private FMOD.Studio.EventInstance instance3;


    [SerializeField] private RectTransform ammoMaskUI;
    [SerializeField] private Vector2 ammoMinMaskUI = new Vector2(0, 184);

    [SerializeField] private Transform aimCursor;
    [SerializeField] private GameObject gun;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private SpriteRenderer gunSprite1;
    [SerializeField] private SpriteRenderer gunSprite2;
    [SerializeField] private GameObject onShootFX;

    [Header("Shoot Bullet")]
    [SerializeField] private GameObject bulletPrefab1;
    [SerializeField] private GameObject bulletPrefab2;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletImpulse1;
    [SerializeField] private float bulletImpulse2;
    [SerializeField] private Vector2 bulletTorque1;
    [SerializeField] private Vector2 bulletTorque2;
    [SerializeField] private float timeBetween2bullets1 = 0.2f;
    [SerializeField] private float timeBetween2bullets2 = 0.2f;
    [SerializeField] private int bulletAmountMax1 = 50;
    [SerializeField] private int bulletAmountMax2 = 50;
    private int bulletAmount1 = 0;
    private int bulletAmount2 = 0;
    private float timerShoot1 = 0.0f;
    private float timerShoot2 = 0.0f;

    [Header("Spray Cone")]
    [SerializeField] private Vector2 coneAngleMinMax = new Vector2(15.0f, 25.0f);
    [SerializeField] private Vector2 sprayForce = new Vector2(0.2f, 0.5f);
    [SerializeField] private float sprayDeplete = 1.0f;

    [Header("Aim Sight")]
    [SerializeField] private float aimSightLength = 10.0f;
    [SerializeField] private Gradient aimSightColor;
    [SerializeField] private LineRenderer lineLeft;
    [SerializeField] private LineRenderer lineRight;

    public bool hasGun1 = false;
    public bool hasGun2 = false;
    public int currentGun = 1;

    float currentConeAngle = 15.0f;

    public bool IsShooting()
    {
        if (hasGun1 && currentGun == 1)
            return timerShoot1 > 0.0f;
        if (hasGun2 && currentGun == 2)
            return timerShoot2 > 0.0f;
        return false;
    }

    private void UpdateAmmoUI()
    {
        if (currentGun == 1)
            ammoMaskUI.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Lerp(ammoMinMaskUI.x, ammoMinMaskUI.y, ((float)bulletAmount1 / (float)bulletAmountMax1)));
        else
            ammoMaskUI.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Lerp(ammoMinMaskUI.x, ammoMinMaskUI.y, ((float)bulletAmount2 / (float)bulletAmountMax2)));
    }

    public bool IsFull()
    {
        return bulletAmount1 == bulletAmountMax1 && bulletAmount2 == bulletAmountMax2;
    }

    public void Refill()
    {
        instance3 = FMODUnity.RuntimeManager.CreateInstance(soundReload);
        instance3.start();
        instance3.release();
        bulletAmount1 = bulletAmountMax1;
        bulletAmount2 = bulletAmountMax2;
        UpdateAmmoUI();
    }

    private void Awake()
    {
        currentConeAngle = coneAngleMinMax.x;

        lineLeft.positionCount = 2;
        lineRight.positionCount = 2;
        lineLeft.colorGradient = aimSightColor;
        lineRight.colorGradient = aimSightColor;

        bulletAmount1 = bulletAmountMax1;
        bulletAmount2 = bulletAmountMax2;
    }

    public void PickupWeapon1()
    {
        hasGun1 = true;
        currentGun = 1;
        gunSprite1.gameObject.SetActive(true);
        gunSprite2.gameObject.SetActive(false);
    }

    public void PickupWeapon2()
    {
        hasGun2 = true;
        currentGun = 2;
        gunSprite2.gameObject.SetActive(true);
        gunSprite1.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("One"))
        {
            if (hasGun1 && currentGun == 2)
            {
                currentGun = 1;
                gunSprite1.gameObject.SetActive(true);
                gunSprite2.gameObject.SetActive(false);
            }
        }
        else if (Input.GetButtonDown("Two"))
        {
            if (hasGun2 && currentGun == 1)
            {
                currentGun = 2;
                gunSprite2.gameObject.SetActive(true);
                gunSprite1.gameObject.SetActive(false);
            }
        }


        if (Input.GetButtonDown("Fire1") || Input.GetButton("Fire1"))
        {
            if (hasGun1 && currentGun == 1)
            {
                ShootGun1();
            }
            else if (hasGun2 && currentGun == 2)
            {
                ShootGun2();
            }
        }
        else
        {
            SprayDeplete();
        }

        UpdateConeLines();

        if (timerShoot1 > 0.0f)
        {
            timerShoot1 -= Time.deltaTime;
        }
        if (timerShoot2 > 0.0f)
        {
            timerShoot2 -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        Vector2 lookDir = aimCursor.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        gun.transform.rotation = Quaternion.Euler(0, 0, angle);

        playerSprite.flipX = lookDir.x < 0;
        gunSprite1.flipY = lookDir.x < 0;
        gunSprite2.flipY = lookDir.x < 0;
    }

    private void ShootGun2()
    {
        if (IsShooting())
            return;

        timerShoot2 += timeBetween2bullets2;

        if (bulletAmount2 <= 0)
        {
            // TODO : sound *clic clic* no more bullets
            instance2 = FMODUnity.RuntimeManager.CreateInstance(soudnUnshoot);
            instance2.start();
            instance2.release();
            return;
        }

        bulletAmount2 -= 1;
        UpdateAmmoUI();

        GameObject bullet = Instantiate(bulletPrefab2, firePoint.position, Quaternion.Euler(0, 0, Random.Range(-360, 360)));
        if (bullet.TryGetComponent<Rigidbody2D>(out Rigidbody2D bulletRb))
        {
            Instantiate(onShootFX, transform.position, onShootFX.transform.rotation);
            Vector3 shootDir = Quaternion.Euler(0, 0, Random.Range(-currentConeAngle, currentConeAngle)) * firePoint.right;
            bulletRb.AddForce(shootDir * bulletImpulse2, ForceMode2D.Impulse);
            int rngRot = Random.Range(0, 2);
            if (rngRot == 0)
                bulletRb.angularVelocity = -Random.Range(bulletTorque2.x, bulletTorque2.y);
            else
                bulletRb.angularVelocity = Random.Range(bulletTorque2.x, bulletTorque2.y);
            Spray();
            instance = FMODUnity.RuntimeManager.CreateInstance(soundShoot);
            instance.start();
            instance.release();
        }
    }

    private void ShootGun1()
    {
        if (IsShooting())
            return;

        timerShoot1 += timeBetween2bullets1;

        if (bulletAmount1 <= 0)
        {
            // TODO : sound *clic clic* no more bullets
            instance2 = FMODUnity.RuntimeManager.CreateInstance(soudnUnshoot);
            instance2.start();
            instance2.release();
            return;
        }

        bulletAmount1 -= 1;
        UpdateAmmoUI();

        GameObject bullet = Instantiate(bulletPrefab1, firePoint.position, Quaternion.Euler(0, 0, Random.Range(-360, 360)));
        if (bullet.TryGetComponent<Rigidbody2D>(out Rigidbody2D bulletRb))
        {
            Instantiate(onShootFX, transform.position, onShootFX.transform.rotation);
            Vector3 shootDir = Quaternion.Euler(0, 0, Random.Range(-currentConeAngle, currentConeAngle)) * firePoint.right;
            bulletRb.AddForce(shootDir * bulletImpulse1, ForceMode2D.Impulse);
            int rngRot = Random.Range(0, 2);
            if (rngRot == 0)
                bulletRb.angularVelocity = -Random.Range(bulletTorque1.x, bulletTorque1.y);
            else
                bulletRb.angularVelocity = Random.Range(bulletTorque1.x, bulletTorque1.y);
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
