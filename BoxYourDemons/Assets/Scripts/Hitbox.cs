using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [Header("Hitbox Settings")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int damageAmount = 10;
    
    private Collider hitboxCollider;

    private void Awake()
    {
        hitboxCollider = GetComponent<Collider>();
        // Make absolutely sure it starts turned off!
        hitboxCollider.enabled = false; 
    }

    public void EnableHitbox()
    {
        hitboxCollider.enabled = true;
    }

    public void DisableHitbox()
    {
        hitboxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((enemyLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            Debug.Log($"BAM! {gameObject.name} hit {other.name} for {damageAmount} damage!");
            
            IDamageable healthScript = other.GetComponentInParent<IDamageable>();
            
            if (healthScript != null)
            {
                healthScript.TakeDamage(damageAmount);
            }

            DisableHitbox(); 
        }
    }
}
