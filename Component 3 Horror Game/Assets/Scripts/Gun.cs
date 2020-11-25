using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private string name;  //name of the specific weapon
    private int magazineCount;  //amount of bullets that can be stored in the gun's magazine. eg a handgun can store a maximum of seven bullets.
    private float reloadTime, firingTime; //the time taken for the gun to be reloaded and the time taken for the next bullet to be avaliable to fire.
    private int damage; //The amount of the damage the gun will deal, handguns will deal less damage compared to a gun like a shotgun.

    public string Name { get => name; set => name = value; }
    public int MagazineCount { get => magazineCount; set => magazineCount = value; }
    public float ReloadTime { get => reloadTime; set => reloadTime = value; }
    public float FiringTime { get => firingTime; set => firingTime = value; }
    public int Damage { get => damage; set => damage = value; }

    public virtual void Fire() { 
        //Handles the code which will be called as soon as the left input button
        // is pressed. This will vary depending on the type of gun, for example, the
        //firing rate should be different
    }
}
