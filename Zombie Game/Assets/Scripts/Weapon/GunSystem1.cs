using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunSystem1 : MonoBehaviour
{
    [Header("Gun Stats")]
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    public Transform bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletLifetime = 3f;

    [Header("Recoil Settings")]
    public Transform gunTransform;
    public Vector3 recoilPosition = new Vector3(0f, -0.05f, -0.1f);
    public float gunRecoilSpeed = 10f;

    public Transform cameraTransform;
    public ParticleSystem muzzleFlash;
    public float cameraRecoilAngle = 2f;
    public float cameraRecoilReturnSpeed = 5f;

    private Vector3 initialGunPosition;
    private Vector3 initialCameraRotation;
    private Vector3 currentCameraRecoil = Vector3.zero;

    [Header("References")]
    public Camera fpsCam;
    public LayerMask whatIsEnemy;
    public TextMeshProUGUI ammunitionDisplay;

    [SerializeField] AudioSource shoot;
    [SerializeField] AudioSource reload;

    public bool isDead = false;

    bool shooting, readyToShoot, reloading;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;

        if (gunTransform != null)
            initialGunPosition = gunTransform.localPosition;

        if (cameraTransform != null)
            initialCameraRotation = cameraTransform.localEulerAngles;
    }

    private void Update()
    {
        if (isDead == true) return;

        HandleInput();

        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText("Ammo: " + bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
    }

    private void LateUpdate()
    {
        if (isDead == true) return;

        if (cameraTransform != null)
        {
            // Smoothly decay recoil offset
            currentCameraRecoil = Vector3.Lerp(currentCameraRecoil, Vector3.zero, cameraRecoilReturnSpeed * Time.deltaTime);

            // Add recoil to current rotation
            cameraTransform.localEulerAngles += currentCameraRecoil;
        }
    }

    private void HandleInput()
    {

        shooting = allowButtonHold ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKey(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
            reload.Play();
        }

        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            Reload();
            reload.Play();
        }


        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
            shoot.Play();
        }
    }

    private void Shoot()
    {
        if (isDead == true) return;

        readyToShoot = false;
        muzzleFlash.Play();

        Ray ray = fpsCam.ScreenPointToRay(
            new Vector3(Screen.width / 2f, Screen.height / 2f, 0)
        );

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, range))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(range);

        Vector3 direction = (targetPoint - bulletSpawn.position).normalized;

        GameObject bullet = Instantiate(
            bulletPrefab.gameObject,
            bulletSpawn.position,
            Quaternion.LookRotation(direction)
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.GetComponent<BulletScript>().damage = damage;

        rb.AddForce(direction * bulletVelocity, ForceMode.Impulse);

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletLifetime));

        bulletsLeft--;
        bulletsShot--;

        if (gunTransform != null)
        {
            StopCoroutine("RecoilGun");
            StartCoroutine(RecoilGun());
        }

        if (cameraTransform != null)
            ApplyCameraRecoil();

        Invoke(nameof(ResetShot), timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke(nameof(Shoot), timeBetweenShots);
    }



    private void ApplyCameraRecoil()
    {
        float recoilX = Random.Range(-cameraRecoilAngle, -cameraRecoilAngle * 0.5f);
        float recoilY = Random.Range(-0.5f, 0.5f);
        currentCameraRecoil += new Vector3(recoilX, recoilY, 0);
    }

    private IEnumerator RecoilGun()
    {
        gunTransform.localPosition += recoilPosition;
        while (Vector3.Distance(gunTransform.localPosition, initialGunPosition) > 0.001f)
        {
            gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, initialGunPosition, gunRecoilSpeed * Time.deltaTime);
            yield return null;
        }
        gunTransform.localPosition = initialGunPosition;
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

    public void Dead()
    {
        isDead = true;
    }
}
