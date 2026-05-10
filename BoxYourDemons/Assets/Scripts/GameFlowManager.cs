using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameFlowManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject levelSetupPanel;
    public GameObject hudPanel;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    [Header("Level Setup UI")]
    public Transform levelListContainer;
    public GameObject levelRowPrefab;
    private List<TMP_Dropdown> activeLevelDropdowns = new List<TMP_Dropdown>();

    [Header("Game References")]
    public EnemyFactory enemyFactory;
    public PlayerHealth playerHealth;
    public Animator playerAnimator;
    public Transform enemySpawnPoint;
    public Transform playerStartLocation;
    public PlayerMovement playerMovement;

    private int currentLevelIndex = 0;
    private GameObject currentActiveEnemy;

    private void Start()
    {
        playerHealth.OnDeath += HandlePlayerDeath;
        
        if (playerMovement != null) playerMovement.InputEnabled = false;
        
        ShowPanel(mainMenuPanel);
    }

    public void Btn_PlayClicked()
    {
        ShowPanel(levelSetupPanel);
        
        foreach (Transform child in levelListContainer) Destroy(child.gameObject);
        activeLevelDropdowns.Clear();
        Btn_AddLevel(); 
    }

    public void Btn_AddLevel()
    {
        GameObject newRow = Instantiate(levelRowPrefab, levelListContainer);
        TMP_Dropdown dropdown = newRow.GetComponentInChildren<TMP_Dropdown>();
        activeLevelDropdowns.Add(dropdown);
    }

    public void Btn_RemoveLevel()
    {
        if (activeLevelDropdowns.Count > 1) 
        {
            TMP_Dropdown lastDropdown = activeLevelDropdowns[activeLevelDropdowns.Count - 1];
            activeLevelDropdowns.Remove(lastDropdown);
            Destroy(lastDropdown.transform.parent.gameObject);
        }
    }

    public void Btn_StartGame()
    {
        ShowPanel(hudPanel);
        currentLevelIndex = 0;
        
        playerHealth.transform.position = playerStartLocation.position;
        playerHealth.Respawn();

	if (playerMovement != null) playerMovement.InputEnabled = true;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	
        SpawnNextEnemy();
    }

    public void Btn_ReturnToMainMenu()
    {
        if (currentActiveEnemy != null) Destroy(currentActiveEnemy);
        ShowPanel(mainMenuPanel);
    }

    private void SpawnNextEnemy()
    {
        if (currentLevelIndex >= activeLevelDropdowns.Count)
        {
            HandleVictory();
            return;
        }

        EnemyDifficulty diff = (EnemyDifficulty)activeLevelDropdowns[currentLevelIndex].value;
        
        currentActiveEnemy = enemyFactory.CreateEnemy(diff, enemySpawnPoint.position);
        
        currentActiveEnemy.GetComponent<EnemyHealth>().OnDeath += HandleEnemyDeath;
    }

    private void HandleEnemyDeath()
    {
        currentLevelIndex++;
        Invoke(nameof(SpawnNextEnemy), 3f); 
    }

    private void HandlePlayerDeath()
    {
        Invoke(nameof(ShowGameOverScreen), 2.5f); 
    }

    private void HandleVictory()
    {
    	if (playerMovement != null) 
        {
            playerMovement.InputEnabled = false;
        }
        if (playerAnimator != null) playerAnimator.SetTrigger("Win");
        Invoke(nameof(ShowVictoryScreen), 3f);
    }

    private void ShowGameOverScreen() => ShowPanel(gameOverPanel);
    private void ShowVictoryScreen() => ShowPanel(victoryPanel);

    private void ShowPanel(GameObject panelToShow)
    {
        mainMenuPanel.SetActive(false);
        levelSetupPanel.SetActive(false);
        hudPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        
        panelToShow.SetActive(true);
        
        if (panelToShow != hudPanel)
        {
            if (playerMovement != null) playerMovement.InputEnabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
