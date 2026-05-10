using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDifficulty
{
    Easy,
    Medium,
    Hard
}

public class EnemyFactory : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject baseEnemyPrefab;

    [Header("Spawn Settings (For Testing)")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private EnemyDifficulty testDifficulty;

    [ContextMenu("Spawn Test Enemy")]
    public void SpawnEnemy()
    {
        CreateEnemy(testDifficulty, spawnPoint.position);
    }

    public GameObject CreateEnemy(EnemyDifficulty difficulty, Vector3 position)
    {
        GameObject newEnemy = Instantiate(baseEnemyPrefab, position, Quaternion.identity);

        EnemyHealth health = newEnemy.GetComponent<EnemyHealth>();
        EnemyAI ai = newEnemy.GetComponent<EnemyAI>();
        CharacterStats stats = newEnemy.GetComponent<CharacterStats>();

        switch (difficulty)
        {
            case EnemyDifficulty.Easy:
                newEnemy.name = "Enemy_Easy";
                stats.attackDamage = 5;
                ai.attackCooldown = 3.0f;
                health.InitializeStats(newHealth: 50, blocks: 2, stunTime: 4.0f);
                break;

            case EnemyDifficulty.Medium:
                newEnemy.name = "Enemy_Medium";
                stats.attackDamage = 15;
                ai.attackCooldown = 2.0f; 
                health.InitializeStats(newHealth: 100, blocks: 3, stunTime: 3.0f);
                break;

            case EnemyDifficulty.Hard:
                newEnemy.name = "Enemy_Hard";
                stats.attackDamage = 30;
                ai.attackCooldown = 1.0f;
                health.InitializeStats(newHealth: 200, blocks: 5, stunTime: 1.5f);
                break;
        }

        return newEnemy;
    }
}
