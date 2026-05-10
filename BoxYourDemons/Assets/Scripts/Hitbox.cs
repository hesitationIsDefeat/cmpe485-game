using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [Header("Hitbox Settings")]
    [SerializeField] private LayerMask targetLayer;
    
    private Collider hitboxCollider;
    
    private Animator ownerAnimator;
    private CharacterStats myStats;

    private void Awake()
    {
        hitboxCollider = GetComponent<Collider>();
        hitboxCollider.enabled = false; 
        
        ownerAnimator = GetComponentInParent<Animator>();
        myStats = GetComponentInParent<CharacterStats>();
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
        if ((targetLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            IDamageable damageableTarget = other.GetComponentInParent<IDamageable>();
            
            if (damageableTarget != null)
            {
            	int damage = myStats != null ? myStats.attackDamage : 10;
                DamageResult hitResult = damageableTarget.TakeDamage(damage);

                if (hitResult == DamageResult.Blocked)
                {
                    EnemyHealth enemyOwner = GetComponentInParent<EnemyHealth>();
                    if (enemyOwner != null)
                    {
                        enemyOwner.OnAttackBlocked();
                    }
                    else 
                    {
                        if (ownerAnimator != null) ownerAnimator.SetTrigger("Recoil");
                    }
                }
            }

            DisableHitbox();
        }
    }
}
