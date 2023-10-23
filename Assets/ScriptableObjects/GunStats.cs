using UnityEngine;

namespace IndividualGames.ScriptableObjects
{
    /// <summary>
    /// Data for gun's stats.
    /// </summary>
    [CreateAssetMenu(fileName = "GunStats", menuName = "IndividualGames/GunStats")]
    public class GunStats : ScriptableObject
    {
        public int AttackDamage = 10;
        public int AmmoCapacity = 10;
        public int CurrentAmmo = 10;
        public bool PierceShot = false;
    }
}