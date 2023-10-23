namespace IndividualGames.DI
{
    /// <summary>
    /// Can take Damage. Game specific damage system.
    /// </summary>
    public interface IDamageable
    {
        public void Damage(int damage);
    }
}