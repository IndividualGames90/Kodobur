using IndividualGames.CaseLib.DI;
using IndividualGames.CaseLib.Signalization;
using IndividualGames.CaseLib.Utils;
using IndividualGames.DI;
using IndividualGames.Game;
using IndividualGames.Pool;
using IndividualGames.ScriptableObjects;
using IndividualGames.UI;
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
        [SerializeField] private GameObject[] _fireVFX;

        [SerializeField] private Transform _muzzleTransform;
        [SerializeField] private EnemyHealthSlider _healthSlider;

        [SerializeField] private bool _isBoss;
        [SerializeField] private bool _isFloating;

        private EnemyStats _enemyStatsPersonal;
        private GameObjectPool _bulletPool;
        private Transform _playerLocation;
        private NavNodeController _navNodeController;

        private bool _attackLocked = false;
        private WaitForSeconds _attackWait = new(.3f);
        private bool _alive = true;

        private ParticleSystem _fireOne;
        private ParticleSystem _fireTwo;

        private void Awake()
        {
            _enemyStatsPersonal = Instantiate(_enemyStats);

            _ai.Init(new Container(new AIParams(this, _animator)));

            _bulletPool = GameController.Instance.EnemyBulletPool;

            _playerLocation = GameController.Instance.PlayerLocation;
            _navNodeController = GameController.Instance.NavNodeController;

            InitializeVFX();
        }

        private void InitializeVFX()
        {
            if (_fireOne == null || _fireTwo == null)
            {
                _fireOne = _fireVFX[0].GetComponent<ParticleSystem>();
                _fireTwo = _fireVFX[1].GetComponent<ParticleSystem>();
                _fireOne.Stop();
                _fireTwo.Stop();
            }
        }

        public void Damage(int damage)
        {
            _enemyStatsPersonal.Health -= damage;
            _healthSlider.UpdateSliderValue((float)_enemyStatsPersonal.Health / (float)_enemyStatsPersonal.HealthMax);

            IsDead();
        }

        /// <summary> Enemy death check. </summary>
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

        /// <summary> Enemy has died. </summary>
        private void Died()
        {
            _alive = false;
            StopAgent();
            EnemyKilled.Emit(_enemyStatsPersonal.ExperienceAward);

            if (_isFloating)
            {
                FloatingDied();
            }

            StartCoroutine(DiedDelay());
        }

        private void FloatingDied()
        {
            _agent.enabled = false;
            var rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = true;
        }

        /// <summary> Delay to show death animation and corpse hang. </summary>
        private IEnumerator DiedDelay()
        {
            yield return new WaitForSeconds(2);
            StopAllCoroutines();
            Destroy(gameObject);
        }

        /// <summary> Set destination towards a given target. Can be forced without checks. </summary>
        public void MoveTowards(Vector3 moveTo, bool forceMove = false)
        {
            if (!_agent.enabled)
            {
                return;
            }

            if (forceMove)
            {
                _agent.SetDestination(moveTo);
            }
            else if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                _agent.SetDestination(moveTo);
            }
        }

        /// <summary> Set destination for player. </summary>
        public void MoveTowardsPlayer()
        {
            MoveTowards(_playerLocation.position, true);
        }

        /// <summary> Set destination for a random navnode. </summary>
        public void MoveTowardsNavNode()
        {
            MoveTowards(_navNodeController.AcquireRandomNavNode());
        }

        /// <summary> Reset navmesh for full stop. </summary>
        public void StopAgent()
        {
            if (_agent.enabled)
            {
                _agent.ResetPath();
            }
        }

        /// <summary> One incremental of rotating towards given target. </summary>
        public void RotateTowards(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, _enemyStatsPersonal.RotationSpeed * Time.deltaTime);
        }

        /// <summary> One incremental of rotating towards player. </summary>
        public void RotateTowardsPlayer()
        {
            RotateTowards(GameController.Instance.PlayerLocation.position);
        }

        /// <summary> Situation check to see player. </summary>
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

        /// <summary> Situation check to attack player. </summary>
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

        /// <summary> Single instance of enemy attack. </summary>
        private IEnumerator Attack()
        {
            _attackLocked = true;

            var attackCount = _isBoss ? 10 : 1;

            Vector3[] radialArray = new Vector3[attackCount];
            var radius = 5f;

            for (int i = 0; i < attackCount; i++)
            {
                float angle = 2 * Mathf.PI * i / attackCount;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius; // Using Z for depth, assuming Y is up

                radialArray[i] = new Vector3(x, 0, z);
            }

            for (int i = 0; i < attackCount; i++)
            {
                var bullet = _bulletPool.Retrieve();
                bullet.GetComponent<BulletController>().Fired(_enemyStatsPersonal.AttackDamage, false, _bulletPool);
                bullet.transform.position = _muzzleTransform.position;
                bullet.transform.forward = _playerLocation.position - _muzzleTransform.position;
                bullet.transform.forward = Quaternion.Euler(radialArray[i]) * bullet.transform.forward;
            }

            yield return _attackWait;
            _attackLocked = false;
        }

        /// <summary> Turn VFX on or off with flag </summary>
        public void ToggleFireVFX(bool firing)
        {
            if (_fireOne == null || _fireTwo == null)
            {
                InitializeVFX();
            }

            if (firing)
            {
                if (!_fireOne.isPlaying || !_fireTwo.isPlaying)
                {
                    _fireOne.Play();
                    _fireTwo.Play();
                }
            }
            else
            {
                _fireOne.Stop();
                _fireTwo.Stop();
            }
        }
    }
}