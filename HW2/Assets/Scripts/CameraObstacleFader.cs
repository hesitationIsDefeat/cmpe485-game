using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObstacleFader : MonoBehaviour
{
    public Transform target; 
    public float fadeAlpha = 0.3f; 

    private Renderer currentlyFadedRenderer;
    private Color originalColor;
    private const string WALL = "Wall";

    void Update()
    {
        Vector3 direction = target.position - transform.position;
        float distance = Vector3.Distance(transform.position, target.position);
        
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, distance))
        {
            if (hit.collider.CompareTag(WALL))
            {
                Renderer hitRenderer = hit.collider.GetComponent<Renderer>();

                if (currentlyFadedRenderer != hitRenderer)
                {
                    ResetFadedWall(); 

                    currentlyFadedRenderer = hitRenderer;
                    originalColor = currentlyFadedRenderer.material.color;

                    Color fadeColor = originalColor;
                    fadeColor.a = fadeAlpha;
                    currentlyFadedRenderer.material.color = fadeColor;
                }
            }
            else
            {
                ResetFadedWall();
            }
        }
        else
        {
            ResetFadedWall();
        }
    }

    private void ResetFadedWall()
    {
        if (currentlyFadedRenderer != null)
        {
            currentlyFadedRenderer.material.color = originalColor;
            currentlyFadedRenderer = null;
        }
    }
}
