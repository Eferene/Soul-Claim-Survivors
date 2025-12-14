public interface IEnemy
{
    void DieEffect();

    void TakeDamage(float damage);
    bool IsDead { get; }
}