using System.Collections.Generic;
using UnityEngine;

public class CameraObstacleFader : MonoBehaviour
{
    public Transform target; 
    public float fadeAlpha = 0.3f; 
    private const string WALL = "Wall";

    private Dictionary<Renderer, Color> fadedWalls = new Dictionary<Renderer, Color>();
    
    private List<Renderer> hitsThisFrame = new List<Renderer>();

    void Update()
    {
        Vector3 direction = target.position - transform.position;
        float distance = Vector3.Distance(transform.position, target.position);
        
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distance);

        hitsThisFrame.Clear();

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag(WALL))
            {
                Renderer hitRenderer = hit.collider.GetComponent<Renderer>();

                if (hitRenderer != null)
                {
                    hitsThisFrame.Add(hitRenderer);

                    if (!fadedWalls.ContainsKey(hitRenderer))
                    {
                        Color originalColor = hitRenderer.material.color;
                        fadedWalls.Add(hitRenderer, originalColor);

                        Color fadeColor = originalColor;
                        fadeColor.a = fadeAlpha;
                        hitRenderer.material.color = fadeColor;
                    }
                }
            }
        }

        List<Renderer> wallsToRestore = new List<Renderer>();
        foreach (Renderer previouslyFadedWall in fadedWalls.Keys)
        {
            if (!hitsThisFrame.Contains(previouslyFadedWall))
            {
                wallsToRestore.Add(previouslyFadedWall);
            }
        }

        foreach (Renderer wallToRestore in wallsToRestore)
        {
            wallToRestore.material.color = fadedWalls[wallToRestore];
            fadedWalls.Remove(wallToRestore);
        }
    }
}