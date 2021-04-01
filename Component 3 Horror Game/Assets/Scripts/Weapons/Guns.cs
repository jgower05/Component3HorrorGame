using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Guns : MonoBehaviour
{
    [SerializeField] private string name;
    [SerializeField] private int magazineCount;
    [SerializeField] private int magazineLimit;
    [SerializeField] private int ammoCount;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private int gunDamage;

    [SerializeField] private float range; 
    [SerializeField] private float reloadTimer; 
    [SerializeField] private float firingCooldownTimer; 
    [SerializeField] private bool rapidFireCapability;
    [SerializeField] private bool hasFired;
    [SerializeField] private bool triggerPulled;
    [SerializeField] private bool needsReloading;
    [SerializeField] private bool reloadingTriggered;
    [SerializeField] private bool isReloading;

    [SerializeField] private Camera fpsCamera;
    [SerializeField] private GunRecoil gunRecoil;
    [SerializeField] private WeaponRecoil weaponRecoil;

    [SerializeField] private string fireSound;
    [SerializeField] private string reloadSound;
    [SerializeField] private string emptySound;
    [SerializeField] private AudioManager audioManager;

    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private GameObject muzzleFlashLight;
    [SerializeField] private Vector3 aimPosition;
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private float adsSpeed = 8f;
    PlayerControls controls;
    bool aimingDownSight;
    public float fireTimer = 0f;

    void Awake() {
        originalPosition = transform.localPosition;
        fireTimer = firingCooldownTimer;

        controls = new PlayerControls();
        controls.Player.Fire.performed += ctx => triggerPulled = true;
        controls.Player.Aim.performed += ctx => aimingDownSight = true;
        controls.Player.Aim.canceled += ctx => aimingDownSight = false;
        controls.Player.Reload.performed += ctx => reloadingTriggered = true;
    }

    void Update() {
        if (aimingDownSight)
        {
            AimDownSight();
        }
        else {
            AimBack();
        }

        if (reloadingTriggered)
        {
            Reload();
        }

        if (triggerPulled && !hasFired && magazineCount > 0 && !reloadingTriggered)
        {
            Fire();
            magazineCount--;
            audioManager.Play(fireSound);
            weaponRecoil.Fire();
            gunRecoil.Fire();
            muzzleFlash.SetActive(true);
            muzzleFlashLight.SetActive(true);
        }
        else if (triggerPulled && magazineCount <= 0) {
            audioManager.Play(emptySound);
            triggerPulled = false;
        }

        if (hasFired) {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                hasFired = false;
                fireTimer = firingCooldownTimer;
                muzzleFlash.SetActive(false);
                muzzleFlashLight.SetActive(false);
            }
        }

        if (isReloading) {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0f)
            {
                isReloading = false;
                reloadingTriggered = false;
            }
        }
    }


    //Allow the gun to fire a projectile
    public void Fire() {
        hasFired = true;
        triggerPulled = false;
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range)) {
            EnemyStats enemy = hit.transform.GetComponent<EnemyStats>();
            if (enemy != null) {
                enemy.TakeDamage(gunDamage);
            }
            //magazineCount--;
        }
    }

    //Allow the gun to be reloaded under certain conditions
    public void Reload() {
        //First check to see if the amount of ammo left is smaller than the maximum magazine size
        isReloading = true;
        if ((ammoCount - magazineLimit) < 0)
        {
            magazineCount = ammoCount;
            ammoCount = 0;
            audioManager.Play(reloadSound);
        }
        else if (magazineCount > 0)
        {
            ammoCount -= magazineLimit - magazineCount;
            magazineCount = magazineLimit;
            audioManager.Play(reloadSound);
        }
        else if (ammoCount <= 0)
        {
            Debug.Log("No ammo left to reload");
        }
        else
        {
            magazineCount = magazineLimit;
            ammoCount -= magazineLimit;
            audioManager.Play(reloadSound);
        }
    }

    //Move the gun to a position to allow the player to aim
    public void AimDownSight() {
        transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * adsSpeed);
    }

    public void AimBack() {
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * adsSpeed);
    }

    void OnEnable() {
        controls.Player.Enable();
    }

    void OnDisable() {
        controls.Player.Disable();
    }
}
