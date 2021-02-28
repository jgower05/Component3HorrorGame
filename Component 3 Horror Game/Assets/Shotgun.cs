using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    public int magazineCount = 5;
    public int ammoCount = 15;
    public float range = 50f;
    public Transform bulletSpawn;
    public bool needsReloading, isReloading;
    public int gunDamage = 24;

    public float firingCooldownTimer = 0f;
    public bool hasFired;
    public float reloadTimer = 0f;
    public bool reloading;

    public Camera fpsCamera;
    public GunRecoil cameraRecoil;
    public WeaponRecoil weaponRecoil;

    public AudioManager audioManager;
    public GameObject muzzleFlash, muzzleFlashLight;
    private Vector3 originalPosition;
    public Vector3 aimPosition;
    public float adsSpeed = 8f;

    // Start is called before the first frame update
    void Start()
    {
        Name = "Shotgun";
        MagazineCount = this.magazineCount;
        ReloadTime = 0.25f;
        FiringTime = 0.15f;
        Damage = gunDamage;

        originalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && this.magazineCount > 0 && !hasFired && !reloading)
        {
            //Fire the gun
            Fire();
            weaponRecoil.Fire();
            cameraRecoil.Fire();
            muzzleFlash.SetActive(true);
            muzzleFlashLight.SetActive(true);
            audioManager.Play("Shotgun Fire");
        }
        else if (Input.GetMouseButtonDown(0) && this.magazineCount <= 0 && !hasFired && !reloading)
        {
            //Guns needs to be reloaded
            //Play a gun empty sound here
            Debug.Log("Guns needs to reload");
            audioManager.Play("Handgun Pin");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        AimDownSight();

        if (hasFired)
        {
            firingCooldownTimer -= Time.deltaTime;
            if (firingCooldownTimer <= 0f)
            {
                hasFired = false;
                muzzleFlash.SetActive(false);
                muzzleFlashLight.SetActive(false);
            }
        }
        if (reloading)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0f)
            {
                reloading = false;
            }
        }
    }

    //Move gun to allow the player to aim down the sight
    private void AimDownSight()
    {
        if (Input.GetMouseButton(1) && !reloading)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * adsSpeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * adsSpeed);
        }
    }

    public override void Fire()
    {
        firingCooldownTimer = 0.5f;
        hasFired = true;
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            EnemyStats enemy = hit.transform.GetComponent<EnemyStats>(); //Retrieve the script from the enemy which handles its 'health'.
            if (enemy != null)
            {
                Debug.Log("Hit!");
                enemy.TakeDamage(gunDamage);
            }
            this.magazineCount--;
        }
    }

    public override void Reload()
    {
        //First check to see if the amount of ammo left is smaller than the maximum magazine size
        reloading = true;
        reloadTimer = 3.7f;
        if ((ammoCount - MagazineCount) < 0)
        {
            this.magazineCount = ammoCount;
            ammoCount = 0;
            audioManager.Play("Shotgun Reload");
        }
        else if (magazineCount > 0)
        {
            ammoCount -= MagazineCount - this.magazineCount;
            this.magazineCount = MagazineCount;
            audioManager.Play("Shotgun Reload");
        }
        else if (ammoCount <= 0)
        {
            Debug.Log("No ammo left to reload");
        }
        else
        {
            this.magazineCount = MagazineCount;
            ammoCount -= MagazineCount;
            audioManager.Play("Shotgun Reload");
        }
    }
}