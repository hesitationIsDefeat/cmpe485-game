using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeDoorController : MonoBehaviour
{
    private const string KEY = "Key";
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(KEY))
        {
            Debug.Log("Key inserted! The Labyrinth is conquered!");
            
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
