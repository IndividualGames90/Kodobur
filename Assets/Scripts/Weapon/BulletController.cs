using IndividualGames.Enemy;
using IndividualGames.Player;
using IndividualGames.Pool;
using IndividualGames.Unity;
using System.Collections;
using UnityEngine;

namespace IndividualGames.Weapon
{
    public class BulletController : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;

        private GameObjectPool _pool;
        private WaitForSeconds _waitPoolReturn = new(20);
        private int _damage;
        private bool _playerOwned;
        private bool _pierceShot;
        private GameObject _lastHitTarget;

        private void FixedUpdate()
        {
            MoveForward();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag(Tags.Player) && !_playerOwned)
            {
                other.GetComponent<PlayerController>().Damage(_damage);
                _pool.ReturnToPool(gameObject);
                return;
            }
            else if (other.transform.CompareTag(Tags.Ground))
            {
                _pool.ReturnToPool(gameObject);
                return;
            }
            else if (other.transform.CompareTag(Tags.Enemy))
            {
                if (!_pierceShot)
                {
                    other.GetComponent<EnemyController>().Damage(_damage);
                    _pool.ReturnToPool(gameObject);
                    return;
                }

                if (_pierceShot && _lastHitTarget == null)
                {
                    _lastHitTarget = other.gameObject;
                    other.GetComponent<EnemyController>().Damage(_damage);
                }
                else if (_pierceShot && !_lastHitTarget.name.Equals(other.name))
                {
                    _lastHitTarget = other.gameObject;
                    other.GetComponent<EnemyController>().Damage(_damage);
                }
            }
        }

        /// <summary> Bullet if fired and in motion. </summary>
        public void Fired(int damage, bool playerOwned, GameObjectPool returnPool, bool pierceShot = false)
        {
            _lastHitTarget = null;
            _playerOwned = playerOwned;
            _pool = returnPool;
            _damage = damage;
            _pierceShot = pierceShot;
            StartCoroutine(PoolCountdown());
        }

        private void MoveForward()
        {
            transform.position += transform.forward * _speed * Time.deltaTime;
        }

        private IEnumerator PoolCountdown()
        {
            yield return _waitPoolReturn;
            _pool.ReturnToPool(gameObject);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}