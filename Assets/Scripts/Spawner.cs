using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;

    public Vector3 spawnPosition = new Vector3(-2, 3, -2);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) {
            SpawnObject();
        }
    }

    void SpawnObject() {
        if (objectToSpawn == null) return;

        Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
    }
}
