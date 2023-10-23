using UnityEngine;

namespace IndividualGames.ScriptableObjects
{
    /// <summary>
    /// Data for gun's stats.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(GunStats), menuName = "IndividualGames/" + nameof(GunStats))]
    public class GunStats : ScriptableObject
    {
        public int AttackDamage = 10;
        public int AmmoCapacity = 10;
        public readonly int StartingAmmoCount = 10;
        public int CurrentAmmo = 10;
        public bool PierceShot = false;
    }
}