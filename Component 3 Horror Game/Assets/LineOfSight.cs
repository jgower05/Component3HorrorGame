﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public float radius;
    [Range(0f, 360f)] public float viewAngle;

    public LayerMask targetLayer;
    public LayerMask obstacleLayer;

    void FindVisibleTargets() {
        Collider[] targets = Physics.OverlapSphere(transform.position, radius, targetLayer);
        for (int i = 0; i < targets.Length; i++)
        {
            Transform target = targets[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2) {
                //float distanceToTarget = Vector3.Distance();
            }
        }
    }


    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}