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

    public Camera fpsCamera;

    // Start is called before the first frame update
    void Start()
    {
        Name = "Handgun";
        MagazineCount = this.magazineCount;
        ReloadTime = 0.25f;
        FiringTime = 0.15f;
        Damage = 12;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && this.magazineCount > 0){
            //Fire the gun
            Fire();
        }
        else if (Input.GetMouseButtonDown(0) && this.magazineCount <= 0) {
            //Guns needs to be reloaded
            //Play a gun empty sound here
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            Reload();
        }
    }

    public override void Fire() {
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range)) {
            Debug.Log(hit.transform.name);
            this.magazineCount--;
        }
    }

    public override void Reload() {
        //First check to see if the amount of ammo left is smaller than the maximum magazine size
        if ((ammoCount - MagazineCount) < 0)
        {
            this.magazineCount = ammoCount;
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
