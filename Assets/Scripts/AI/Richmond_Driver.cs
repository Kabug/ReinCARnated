using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Richmond_Driver : MonoBehaviour
{
    [SerializeField] private Transform movePositionTransform;
    private NavMeshAgent navMeshAgent;

    private void Awake(){
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        navMeshAgent.destination = movePositionTransform.position;
    }
}