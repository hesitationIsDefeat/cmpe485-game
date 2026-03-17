using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionAreaController : MonoBehaviour
{
    private const string PLAYER = "Player";
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("VisionArea cannot find the GameManager in the scene!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER))
        {
            if (gameManager) {
                gameManager.ShowEndScreen(); 
            }
        }
    }
}
