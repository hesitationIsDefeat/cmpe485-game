using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeDoorController : MonoBehaviour
{
    private const string KEY = "Key";
    public GameManager gameManager;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(KEY))
        {
            if (gameManager) {
                gameManager.ShowEndScreen();
            }
        }
    }
}
