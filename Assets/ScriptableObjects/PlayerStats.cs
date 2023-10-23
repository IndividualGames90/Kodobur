using UnityEngine;

namespace IndividualGames.ScriptableObjects
{
    /// <summary>
    /// Data for player's stats.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(PlayerStats), menuName = "IndividualGames/" + nameof(PlayerStats))]
    public class PlayerStats : ScriptableObject
    {
        /// <summary> Current player health. </summary>
        public int Health = 70;
        /// <summary> Level assigned maximum player health. </summary>
        public int HealthMaximum = 100;

        /// <summary> Current bullet count. </summary>
        public int BulletCount = 10;
        /// <summary> Maximum bullet capacity. </summary>
        public const int BulletCountMaximum = 100;

        /// <summary> Current killed enemy count. </summary>
        public int KillScore = 0;
        /// <summary> Current achieved level. </summary>
        public int Level = 1;
        /// <summary> Current experience point collected. </summary>
        public int ExperiencePoints = 0;
    }
}