using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PatrolRoute
{
    public Transform startPoint;
    public Transform endPoint;
}

public class GuardSpawner : MonoBehaviour
{
[Header("Guard Settings")]
    public GameObject guardPrefab;
    
    [Header("Route Locations")]
    public List<PatrolRoute> patrolRoutes = new List<PatrolRoute>();  

    void Start()
    {
        foreach (PatrolRoute route in patrolRoutes)
        {
            SpawnGuard(route);
        }
    }

    public void SpawnGuard(PatrolRoute route)
    {
        if (route.startPoint == null || route.endPoint == null)
        {
            Debug.LogWarning("A patrol route is missing a start or end point. Skipping this guard.");
            return;
        }

        Vector3 lookDirection = route.endPoint.position - route.startPoint.position;
        
        if (lookDirection == Vector3.zero) 
        {
            lookDirection = Vector3.forward;
        }

        lookDirection.y = 0f; 
        Quaternion startingRotation = Quaternion.LookRotation(lookDirection);

        GameObject newGuard = Instantiate(guardPrefab, route.startPoint.position, startingRotation);

        GuardController controller = newGuard.GetComponent<GuardController>();

        if (controller != null)
        {
            controller.Initialize(route.startPoint, route.endPoint);
        }
        else
        {
            Debug.LogError("The spawned guard is missing a GuardController script!");
        }
    }
}
