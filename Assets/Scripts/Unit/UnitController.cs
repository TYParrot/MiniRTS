using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    private GameObject unitMarker;
    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        
    }

    public void SelectUnit()
    {
        unitMarker.SetActive(true);
    }

    public void DeselectUnit()
    {
        unitMarker.SetActive(false);
    }

    public void MoveTo(Vector2 end)
    {
        navMeshAgent.SetDestination(end);
    }
}
