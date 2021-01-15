using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointIdentifier : MonoBehaviour
{

    public float radius;
    [Range(0f, 360f)] public float viewAngle;
    public LayerMask waypointLayer;
    public LayerMask obstacleLayer;

    public List<Transform> visibleWaypoints = new List<Transform>();
    public bool haveWaypointsBeenIdentified;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleWaypoints();
        }
    }

    void FindVisibleWaypoints() {
        visibleWaypoints.Clear();
        Collider[] waypoints = Physics.OverlapSphere(transform.position, radius, waypointLayer); //Stores all objects within the radius that has the waypoint kayer attached to it.
        for (int i = 0; i < waypoints.Length; i++)
        {
            Transform waypoint = waypoints[i].transform;
            Vector3 directionToWaypoint = (waypoint.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToWaypoint) < viewAngle / 2) { //Makes sure the waypoint is within the viewing angle. 
                float distanceToWaypoint = Vector3.Distance(transform.position, waypoint.position);
                if (!Physics.Raycast(transform.position, directionToWaypoint, distanceToWaypoint, obstacleLayer))
                {
                    //UnityEngine.Debug.Log("Waypoint identified!");
                    visibleWaypoints.Add(waypoint);
                    haveWaypointsBeenIdentified = true;
                }
            }
        }
        if (waypoints == null) {
            haveWaypointsBeenIdentified = false;
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
