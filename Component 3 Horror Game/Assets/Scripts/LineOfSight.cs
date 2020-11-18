using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public float radius;
    [Range(0f, 360f)] public float viewAngle;

    public LayerMask targetLayer;
    public LayerMask waypointLayer; //Reference to the waypoint layer -> allows the AI to differentiate the objects in its line of sight (LOS)
    public LayerMask obstacleLayer;

    public List<Transform> visibleTargets = new List<Transform>();
    public List<Transform> visibleWaypoints = new List<Transform>(); //stores references to any waypoints located within the LOS

    void Start() {
        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    IEnumerator FindTargetsWithDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets() {
        visibleTargets.Clear();
        Collider[] waypoints = Physics.OverlapSphere(transform.position, radius, waypointLayer); //Another array created to store any waypoint instances
        Collider[] targets = Physics.OverlapSphere(transform.position, radius, targetLayer);
        for (int i = 0; i < targets.Length; i++)
        {
            Transform target = targets[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2) {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleLayer)) {
                    visibleTargets.Add(target);
                }
            }
        }
        for (int i = 0; i < waypoints.Length; i++) //Goes through the waypoint array
        {
            Transform waypoint = waypoints[i].transform;
            Vector3 directionToWaypoint = (waypoint.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToWaypoint) < viewAngle / 2) { //Check if the waypoint is in the line of sight
                float distanceToWaypoint = Vector3.Distance(transform.position, waypoint.position);
                if (!Physics.Raycast(transform.position, directionToWaypoint, distanceToWaypoint, waypointLayer)) { //Make sure no obstacles blocking waypoint
                    visibleWaypoints.Add(waypoint);  
                }
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
