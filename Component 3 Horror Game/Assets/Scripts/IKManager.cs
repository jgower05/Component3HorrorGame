using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKManager : MonoBehaviour
{
    public Transform target;
    public Transform newTarget;
    public float distanceThreshold;
    public float speed = 0.5f;
    public float t;

    public bool isGrounded;
    public float radius;
    public LayerMask groundLayer;
    public float distance;

    public IKManager oppositeLeg;

    public bool isMoving;

    //Called before the first frame is called
    void Start() {
        distanceThreshold = Random.Range(0.2f, 0.3f);
    }

    void FixedUpdate() {
        //Checks every frame to see if the leg can move or not
        if (oppositeLeg.isMoving == false && this.isMoving == false){
            TryMove();
        }
    }

    public void TryMove() {
        float distance = Vector3.Distance(target.position, newTarget.position);
        float step = speed * Time.deltaTime;
        isGrounded = Physics.SphereCast(target.position, radius, Vector3.down, out RaycastHit hit, groundLayer);
        if (distance >= distanceThreshold){
            isMoving = true;
            StartCoroutine(Move());
        }
    }

    public IEnumerator Move() {
        Vector3 centrePoint = (target.position + newTarget.position) / 2;
        centrePoint += (newTarget.up * (Vector3.Distance(target.position, newTarget.position) / 2f)) + new Vector3(0f, 0.15f, 0f);
        //Debug.Log(centrePoint);
        t = 0.0f;
        while (t < 1) {
            t += Time.deltaTime * speed;
            target.position = Mathf.Pow(1 - t, 2) * target.position + 2 * (1 - t) * t * centrePoint + Mathf.Pow(t, 2) * newTarget.position;
            yield return null;
        }
        isMoving = false;
    }

}