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

        private void FixedUpdate()
        {
            MoveForward();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag(Tags.Player) && !_playerOwned)
            {
                other.GetComponent<PlayerController>().Damage(_damage);
            }
            else if (other.transform.CompareTag(Tags.Ground))
            {
                //TODO: PLAY VFX
            }
            else if (other.transform.CompareTag(Tags.Enemy))
            {
                other.GetComponent<EnemyController>().Damage(_damage);
            }

            if (_pool != null)
            {
                _pool.ReturnToPool(gameObject);
            }
            else
            {
                Debug.Log($"Pool empty");
            }
        }

        /// <summary> Bullet if fired and in motion. </summary>
        public void Fired(int damage, bool playerOwned, GameObjectPool returnPool)
        {
            _playerOwned = playerOwned;
            _pool = returnPool;
            _damage = damage;
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