using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionAreaController : MonoBehaviour
{
    public const string PLAYER = "Player";
    public GameManager gameManager;

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
