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
        public int AttackDamage = 10;
        public int ExperienceAward = 1;
    }
}