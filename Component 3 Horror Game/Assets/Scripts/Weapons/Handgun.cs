using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handgun : Gun
{
    public int magazineCount = 7;
    public int ammoCount = 35;
    public float range = 100f;
    public Transform bulletSpawn;
    public bool needsReloading, isReloading;
    public int gunDamage = 12;

    public float firingCooldownTimer = 0f;
    public bool hasFired;
    public float reloadTimer = 0f;
    public bool reloading;

    public Camera fpsCamera;
    public GunRecoil cameraRecoil;
    public WeaponRecoil weaponRecoil;

    public AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        Name = "Handgun";
        MagazineCount = this.magazineCount;
        ReloadTime = 0.25f;
        FiringTime = 0.15f;
        Damage = gunDamage;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && this.magazineCount > 0 && !hasFired && !reloading){
            //Fire the gun
            Fire();
            weaponRecoil.Fire();
            cameraRecoil.Fire();
            audioManager.Play("Handgun Fire");
        }
        else if (Input.GetMouseButtonDown(0) && this.magazineCount <= 0 && !hasFired && !reloading) {
            //Guns needs to be reloaded
            //Play a gun empty sound here
            Debug.Log("Guns needs to reload");
            audioManager.Play("Handgun Pin");
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            Reload();
            audioManager.Play("Handgun Reload");
        }

        if (hasFired) {
            firingCooldownTimer -= Time.deltaTime;
            if (firingCooldownTimer <= 0f) {
                hasFired = false;
            }
        }
        if (reloading) {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0f) {
                reloading = false;
            }
        }
    }

    public override void Fire() {
        firingCooldownTimer = 0.35f;
        hasFired = true;
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range)) {
            Debug.Log(hit.transform.name);
            EnemyStats enemy = hit.transform.GetComponent<EnemyStats>(); //Retrieve the script from the enemy which handles its 'health'.
            if (enemy != null) { 
                Debug.Log("Hit!");
                enemy.TakeDamage(gunDamage);
            }
            this.magazineCount--;
        }
    }

    public override void Reload() {
        //First check to see if the amount of ammo left is smaller than the maximum magazine size
        reloading = true;
        reloadTimer = 2f;
        if ((ammoCount - MagazineCount) < 0)
        {
            this.magazineCount = ammoCount;
            ammoCount = 0;
        }
        else if (magazineCount > 0) {
            ammoCount -= MagazineCount - this.magazineCount;
            this.magazineCount = MagazineCount;
        }
        else if (ammoCount <= 0)
        {
            Debug.Log("No ammo left to reload");
        }
        else
        {
            this.magazineCount = MagazineCount;
            ammoCount -= MagazineCount;
        }
    }
}
