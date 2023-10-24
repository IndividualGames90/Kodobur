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
        private Transform _playerLocation;
        private NavNodeController _navNodeController;

        private bool _attackLocked = false;
        private WaitForSeconds _attackWait = new(.3f);
        private bool _alive = true;

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

            IsDead();
        }

        public bool IsDead()
        {
            if (_alive && _enemyStatsPersonal.Health <= 0)
            {
                Died();
                return true;
            }
            else if (_enemyStatsPersonal.Health <= 0)
            {
                return true;
            }
            return false;
        }

        private void Died()
        {
            _alive = false;
            StopAgent();
            EnemyKilled.Emit(_enemyStatsPersonal.ExperienceAward);
            StartCoroutine(DiedDelay());
        }

        private IEnumerator DiedDelay()
        {
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
        }

        public void MoveTowards(Vector3 moveTo, bool forceMove = false)
        {
            if (forceMove)
            {
                _agent.SetDestination(moveTo);
            }
            else if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                _agent.SetDestination(moveTo);
            }
        }

        public void MoveTowardsPlayer()
        {
            MoveTowards(_playerLocation.position, true);
        }

        public void MoveTowardsNavNode()
        {
            MoveTowards(_navNodeController.AcquireRandomNavNode());
        }

        public void StopAgent()
        {
            _agent.ResetPath();
        }

        private bool IsAgentAtTarget(Vector3 targetPoint, float tolerance = 0.1f)
        {
            return (_agent.transform.position - targetPoint).sqrMagnitude < tolerance * tolerance;
        }

        public void RotateTowards(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, _enemyStatsPersonal.RotationSpeed * Time.deltaTime);
        }

        public void RotateTowardsPlayer()
        {
            RotateTowards(GameController.Instance.PlayerLocation.position);
        }

        public bool CanSpotPlayer()
        {
            if ((GameController.Instance.PlayerLocation.position - transform.position).sqrMagnitude
                < _enemyStatsPersonal.SpotDistanceMax * _enemyStatsPersonal.SpotDistanceMax)
            {
                return true;
            }

            var ray = new Ray(transform.position, GameController.Instance.PlayerLocation.position - transform.position);
            return Raycaster.HitPlayer(ray, _enemyStatsPersonal.SpotDistanceMax).Item1;
        }

        public bool CanAttackPlayer()
        {
            if ((GameController.Instance.PlayerLocation.position - transform.position).sqrMagnitude
                < _enemyStatsPersonal.AttackDistanceMax * _enemyStatsPersonal.AttackDistanceMax)
            {
                if (!_attackLocked)
                {
                    StartCoroutine(Attack());
                }
                return true;
            }

            var ray = new Ray(transform.position, GameController.Instance.PlayerLocation.position);
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
            bullet.transform.forward = _playerLocation.position - _muzzleTransform.position;

            yield return _attackWait;
            _attackLocked = false;
        }
    }
}