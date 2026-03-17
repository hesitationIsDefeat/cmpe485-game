using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
[Header("Trap Settings")]
    public float safeTime = 2f;      
    public float warningTime = 1f;   
    public float dangerTime = 1.5f; 

private const string PLAYER = "Player";
    public Color safeColor = Color.white;
    public Color dangerColor = Color.red;

    private Renderer trapRenderer;
    private bool isDangerous = false;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
        trapRenderer = GetComponent<Renderer>();
        
        StartCoroutine(TrapCycle());
    }

    IEnumerator TrapCycle()
    {
        while (true)
        {
            isDangerous = false;
            trapRenderer.material.color = safeColor;
            yield return new WaitForSeconds(safeTime);

            float elapsed = 0f;
            while (elapsed < warningTime)
            {
                trapRenderer.material.color = Color.Lerp(safeColor, dangerColor, elapsed / warningTime);
                elapsed += Time.deltaTime;
                yield return null; 
            }

            trapRenderer.material.color = dangerColor;
            isDangerous = true;
            yield return new WaitForSeconds(dangerTime);

            elapsed = 0f;
            while (elapsed < warningTime)
            {
                trapRenderer.material.color = Color.Lerp(dangerColor, safeColor, elapsed / warningTime);
                elapsed += Time.deltaTime;
                yield return null; 
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (isDangerous && other.CompareTag(PLAYER))
        {
            if (gameManager != null)
            {
                gameManager.ShowEndScreen();
            }
        }
    }
}
