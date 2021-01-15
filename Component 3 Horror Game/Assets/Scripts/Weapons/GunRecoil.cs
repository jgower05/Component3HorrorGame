using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public float rotationSpeed = 6f;
    public float returnSpeed = 25f;
    public Vector3 recoilRotation = new Vector3(2f,2f,2f);

    private Vector3 currentRotation;
    public Vector3 rot;

    void FixedUpdate() {
        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        rot = Vector3.Slerp(rot, currentRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    public void Fire() {
        currentRotation += new Vector3(-recoilRotation.x, Random.Range(-recoilRotation.y, recoilRotation.y), Random.Range(-recoilRotation.z, recoilRotation.z));
    }

}
