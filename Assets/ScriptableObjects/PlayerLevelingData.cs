using UnityEngine;

namespace IndividualGames.ScriptableObjects
{
    /// <summary>
    /// Leveling grading for each level step.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(PlayerLevelingData), menuName = "IndividualGames/" + nameof(PlayerLevelingData))]
    public class PlayerLevelingData : ScriptableObject
    {
        /// <summary> Experience required for next level achievement. </summary>
        public int[] LevelingGrade;
    }
}