interface IDamageable
{
    int Life { get; }

    int MaxLife { get; }

    void TakeDamage(int amount);
}