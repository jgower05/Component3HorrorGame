using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectorAI : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject enemy;
    private LineOfSight lineOfSight;
    public float menaceGauge;

}
