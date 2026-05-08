public enum DamageResult
{
    Success, 
    Blocked,
    Ignored  
}

public interface IDamageable
{
    DamageResult TakeDamage(int damage);
}
