using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{

    [SerializeField] Transform target;
    NavMeshAgent test;
    void Start()
    {
        test = GetComponent<NavMeshAgent>();
    }

    
    void Update()
    {
        test.destination = target.position;
    }
}
