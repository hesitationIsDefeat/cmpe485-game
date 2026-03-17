using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSpawner : MonoBehaviour
{
[Header("Guard Settings")]
    public GameObject guardPrefab;
    
    [Header("Route Locations")]
    public Transform location0;
    public Transform location1;   

    void Start()
    {
        SpawnGuard();
    }

    public void SpawnGuard()
    {
        Vector3 lookDirection = location1.position - location0.position;
        lookDirection.y = 0f; 
        Quaternion startingRotation = Quaternion.LookRotation(lookDirection);

        GameObject newGuard = Instantiate(guardPrefab, location0.position, startingRotation);

        // GuardController controller = newGuard.GetComponent<GuardController>();

        // if (controller != null)
        // {
        //     controller.Initialize(location0, location1);
        // }
        // else
        // {
        //     Debug.LogError("The spawned guard is missing a GuardController script!");
        // }
    }
}
