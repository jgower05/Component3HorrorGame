using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handgun : Gun
{
    public int magazineCount = 7;
    public int ammoCount = 35;

    // Start is called before the first frame update
    void Start()
    {
        Name = "Handgun";
        MagazineCount = 7;
        ReloadTime = 0.25f;
        FiringTime = 0.15f;
        Damage = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            //Fire the gun
            Debug.Log("The gun has fired! Pew pew!");
        }
    }
}
