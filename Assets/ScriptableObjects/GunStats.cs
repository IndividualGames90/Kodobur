using UnityEngine;

namespace IndividualGames.ScriptableObjects
{
    /// <summary>
    /// Data for gun's stats.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(GunStats), menuName = "IndividualGames/" + nameof(GunStats))]
    public class GunStats : ScriptableObject
    {
        /// <summary> Damage delivered on attack. </summary>
        public int AttackDamage = 10;

        /// <summary> Current amount of carried bullets. </summary>
        public int BulletCarried = 10;
        /// <summary> Maximum bullet capacity to be carried. </summary>
        public int BulletCapacity = 100;

        /// <summary> Special attack type that goes through enemies. </summary>
        public bool PierceShot = false;
        /// <summary> Special attack type for three bullets in one. </summary>
        public bool TripleShot = false;

    }
}