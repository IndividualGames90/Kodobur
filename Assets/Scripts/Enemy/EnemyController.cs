using IndividualGames.CaseLib.DI;
using IndividualGames.CaseLib.Signalization;
using IndividualGames.CaseLib.Utils;
using IndividualGames.DI;
using IndividualGames.Game;
using IndividualGames.Pool;
using IndividualGames.ScriptableObjects;
using IndividualGames.Weapon;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace IndividualGames.Enemy
{
    /// <summary>
    /// Root controller for enemy units.
    /// </summary>
    public class EnemyController : MonoBehaviour, IDamageable
    {
        public static readonly BasicSignal<int> EnemyKilled = new();

        [SerializeField] private EnemyStats _enemyStats;
        [SerializeField] private AI _ai;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;

        [SerializeField] private Transform _muzzleTransform;

        private EnemyStats _enemyStatsPersonal;
        private GameObjectPool _bulletPool;
        private Vector3 _playerLocation;
        private NavNodeController _navNodeController;

        private bool _attackLocked = false;
        private WaitForSeconds _attackWait = new(.1f);

        private void Awake()
        {
            _enemyStatsPersonal = Instantiate(_enemyStats);

            _ai.Init(new Container(new AIParams(this, _animator)));

            _bulletPool = GameController.Instance.EnemyBulletPool;

            _playerLocation = GameController.Instance.PlayerLocation;
            _navNodeController = GameController.Instance.NavNodeController;
        }


        public void Damage(int damage)
        {
            _enemyStatsPersonal.Health -= damage;

            if (_enemyStatsPersonal.Health <= 0)
            {
                Died();
            }
        }

        public bool IsDead()
        {
            if (_enemyStatsPersonal.Health <= 0)
            {
                Died();
                return true;
            }
            return false;
        }

        private void Died()
        {
            EnemyKilled.Emit(_enemyStatsPersonal.ExperienceAward);
            StartCoroutine(DiedDelay());
        }

        private IEnumerator DiedDelay()
        {
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
        }

        public void MoveTowards(Vector3 moveTo)
        {
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                _agent.SetDestination(moveTo);
            }
        }

        public void MoveTowardsPlayer()
        {
            MoveTowards(_playerLocation);
        }

        public void MoveTowardsNavNode()
        {
            MoveTowards(_navNodeController.AcquireRandomNavNode());
        }

        public void RotateTowards(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, _enemyStatsPersonal.RotationSpeed * Time.deltaTime);
        }

        public bool CanSpotPlayer()
        {
            var ray = new Ray(transform.position, GameController.Instance.PlayerLocation);
            return Raycaster.HitPlayer(ray, _enemyStatsPersonal.SpotDistanceMax).Item1;
        }

        public bool CanAttackPlayer()
        {
            var ray = new Ray(transform.position, GameController.Instance.PlayerLocation);
            var canAttack = Raycaster.HitPlayer(ray, _enemyStatsPersonal.AttackDistanceMax).Item1;

            if (canAttack && !_attackLocked)
            {
                StartCoroutine(Attack());
            }

            return canAttack;
        }

        private IEnumerator Attack()
        {
            _attackLocked = true;

            var bullet = _bulletPool.Retrieve();
            bullet.GetComponent<BulletController>().Fired(_enemyStatsPersonal.AttackDamage, true, _bulletPool);
            bullet.transform.position = _muzzleTransform.position;
            bullet.transform.forward = _muzzleTransform.forward;

            yield return _attackWait;
            _attackLocked = false;
        }
    }
}