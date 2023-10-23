using IndividualGames.Pool;
using IndividualGames.Unity;
using UnityEngine;

namespace IndividualGames.Weapon
{
    public class BulletController : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;

        private GameObjectPool _pool;
        private bool _playerOwned;

        private void FixedUpdate()
        {
            MoveForward();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag(Tags.Player) && !_playerOwned)
            {

            }
            else if (other.transform.CompareTag(Tags.Ground))
            {

            }
            else if (other.transform.CompareTag(Tags.Enemy))
            {

            }

            _pool.ReturnToPool(gameObject);
        }

        /// <summary> Bullet if fired and in motion. </summary>
        public void Fired(bool playerOwned, GameObjectPool returnPool)
        {
            _playerOwned = playerOwned;
            _pool = returnPool;
        }

        private void MoveForward()
        {
            transform.position += transform.forward * _speed * Time.deltaTime;
        }
    }
}