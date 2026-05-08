using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [Header("Hitbox Settings")]
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private int damageAmount = 10;
    
    private Collider hitboxCollider;
    
    private Animator ownerAnimator;

    private void Awake()
    {
        hitboxCollider = GetComponent<Collider>();
        hitboxCollider.enabled = false; 
        
        ownerAnimator = GetComponentInParent<Animator>();
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
                DamageResult hitResult = damageableTarget.TakeDamage(damageAmount);

                if (hitResult == DamageResult.Blocked)
                {
                    if (ownerAnimator != null)
                    {
                        ownerAnimator.SetTrigger("Recoil");
                    }
                }
            }

            DisableHitbox();
        }
    }
}
