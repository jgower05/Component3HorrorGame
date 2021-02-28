using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private GameObject light;
    public float timer;
    public float minimumTime, maximumTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FlickerLight());
    }

    IEnumerator FlickerLight() {
        light.SetActive(true);
        timer = Random.Range(minimumTime, maximumTime);
        yield return new WaitForSeconds(timer);
        light.SetActive(false);
        timer = Random.Range(minimumTime, maximumTime);
        yield return new WaitForSeconds(timer);
        StartCoroutine(FlickerLight());
    }
}
