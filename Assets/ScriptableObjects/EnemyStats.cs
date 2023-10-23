using UnityEngine;

namespace IndividualGames.ScriptableObjects
{
    /// <summary>
    /// Data for enemy's stats.
    /// </summary>
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "IndividualGames/EnemyStats")]
    public class EnemyStats : ScriptableObject
    {
        public int Health = 30;
        public const int AttackDamage = 10;
        public const int ExperienceAward = 1;
    }
}