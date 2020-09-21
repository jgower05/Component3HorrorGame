using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastNewTarget : MonoBehaviour
{
    public float distance;
    public LayerMask groundLayer;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 targetLocation;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distance, groundLayer) || Physics.Raycast(transform.position, Vector3.up, out hit, distance, groundLayer)) {
            //Debug.Log("Adjust!");
            transform.position = hit.point;
        }
    }
}
