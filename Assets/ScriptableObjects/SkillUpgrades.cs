using UnityEngine;

namespace IndividualGames.ScriptableObjects
{
    /// <summary>
    /// Skill Upgrades levels of the player and gun talents.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(SkillUpgrades), menuName = "IndividualGames/SkillUpgrades")]
    public class SkillUpgrades : ScriptableObject
    {
        #region Player Talents
        public int PlayerWalkSpeed;
        public int PlayerJumpHeight;
        public int PlayerHealthMax;
        #endregion

        #region Gun Talents
        public int GunDamage;
        public int GunAmmoMax;
        public bool PierceShot;
        public bool TripleShot;
        #endregion
    }
}