using IndividualGames.CaseLib.DataStructures;
using IndividualGames.Enemy;
using IndividualGames.Player;
using IndividualGames.Pool;
using UnityEngine;

namespace IndividualGames.Game
{
    /// <summary>
    /// Controls the state of the game. Similar to a primitive registry.
    /// </summary>
    public class GameController : SingletonBehavior<GameController>
    {
        public Vector3 PlayerLocation => _playerController.transform.position;
        [SerializeField] private PlayerController _playerController;

        public GameObjectPool EnemyBulletPool => _enemyBulletPool;
        [SerializeField] private GameObjectPool _enemyBulletPool;

        public NavNodeController NavNodeController => _navNodeController;
        [SerializeField] private NavNodeController _navNodeController;
    }
}