using UnityEngine;

namespace IndividualGames.ScriptableObjects
{
    /// <summary>
    /// Skill Upgrades levels of the player and gun talents.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(SkillUpgrades), menuName = "IndividualGames/SkillUpgrades")]
    public class SkillUpgrades : ScriptableObject
    {
        public int SkillPoints;

        #region Player Talents
        public int PlayerWalkSpeed;
        public int PlayerJumpHeight;
        public int PlayerHealthMax;
        #endregion

        #region Gun Talents
        public int GunDamage;
        public int GunAmmoMax;
        public bool PierceShot = false;
        public bool TripleShot = false;
        #endregion

        #region Caps
        public int SkillCap = 5;
        #endregion

        #region Required Points
        public int RequiredPointsForInt = 1;
        public int RequiredPointsForBool = 2; ///DevNote: Made this 2 for easier testing.
        #endregion
    }
}