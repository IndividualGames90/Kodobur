namespace IndividualGames.DI
{
    /// <summary>
    /// Can take Damage. Game specific damage system.
    /// </summary>
    public interface IDamageable
    {
        /// <summary> Perform damage on this object. </summary>
        public void Damage(int damage);
    }
}