using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Cryptography;
using UnityEngine;

public class TerrainMatcher : MonoBehaviour
{

    public float length = 5f;
    public LayerMask groundLayer;
    public bool isTerrain;
    RaycastHit hit;

    void FixedUpdate()
    {
        //Shoots raycast of length 5 downwards from the position of the gameobject this script is attached to, if it hits the ground it returns the variable true.  
        if (Physics.Raycast(transform.position + new Vector3(0f, 1f, 0f), -Vector3.up, out hit, length, groundLayer)) {
            isTerrain = true;
            //UnityEngine.Debug.DrawLine(transform.position, hit.point, Color.red, length);
            //Move sphere to match terrain position
            transform.position = hit.point;
            //UnityEngine.Debug.Log("Terrain found!");
        }
    }
}
