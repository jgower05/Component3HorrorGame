using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        transform.LookAt(targetPosition);

        float step = 2f * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.position, step);
    }
}
