using IndividualGames.CaseLib.Signalization;
using IndividualGames.DI;
using IndividualGames.ScriptableObjects;
using UnityEngine;

namespace IndividualGames.Enemy
{
    /// <summary>
    /// Root controller for enemy units.
    /// </summary>
    public class EnemyController : MonoBehaviour, IDamageable
    {
        public static readonly BasicSignal<int> EnemyKilled = new();

        [SerializeField] private EnemyStats _enemyStats;

        private EnemyStats _enemyStatsPersonal;

        private void Awake()
        {
            _enemyStatsPersonal = Instantiate(_enemyStats);
        }

        public void Damage(int damage)
        {
            _enemyStatsPersonal.Health -= damage;

            if (_enemyStatsPersonal.Health <= 0)
            {
                Died();
            }
        }

        private void Died()
        {
            EnemyKilled.Emit(_enemyStatsPersonal.ExperienceAward);
            Destroy(gameObject);
        }
    }
}