using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Legs")]
    [SerializeField] Transform[] legs;

    [Header("Body")]
    [SerializeField] Transform body;

    public bool firstSetOfLegsMoving = true;
    public bool secondSetOfLegsMoving = false;
    float timeToWalk = 0f;
    [SerializeField] float bodyOffset = 4f;

    public Transform player;
    [SerializeField] float offset;
    [SerializeField] float enemySpeed;

    Rigidbody rb;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void LateUpdate() {
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), enemySpeed * Time.deltaTime);
        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(targetPosition); 
    }

    private Vector3 UpdateBodyPosition() { //Update the body position based on the average y position of the 8 legs. 
        float averageY = 0f;
        for (int i = 0; i < legs.Length; i++)
        {
            averageY += legs[i].position.y;
        }
        return new Vector3(body.position.x, (averageY / legs.Length) + bodyOffset, body.position.z);

    }
}
