using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(WaypointIdentifier))]
public class FieldOfViewWaypoint : Editor
{
    void OnSceneGUI()
    {
        WaypointIdentifier los = (WaypointIdentifier)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(los.transform.position, Vector3.up, Vector3.forward, 360, los.radius);
        Vector3 viewAngleA = los.DirFromAngle(-los.viewAngle / 2, false);
        Vector3 viewAngleB = los.DirFromAngle(los.viewAngle / 2, false);

        Handles.DrawLine(los.transform.position, los.transform.position + viewAngleA * los.radius);
        Handles.DrawLine(los.transform.position, los.transform.position + viewAngleB * los.radius);

        Handles.color = Color.red;
        foreach (Transform visibleWaypoint in los.visibleWaypoints)
        {
            Handles.DrawLine(los.transform.position, visibleWaypoint.position);
        }
    }
}
