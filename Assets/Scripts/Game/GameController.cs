using IndividualGames.CaseLib.DataStructures;
using IndividualGames.CaseLib.Signalization;
using IndividualGames.CaseLib.UI;
using IndividualGames.Enemy;
using IndividualGames.Player;
using IndividualGames.Pool;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IndividualGames.Game
{
    /// <summary>
    /// Controls the state of the game. Similar to a primitive registry.
    /// </summary>
    public class GameController : SingletonBehavior<GameController>
    {
        /// <summary> Emit to signify game over. </summary>
        public readonly BasicSignal GameEnded = new();

        public Transform PlayerLocation => _playerController.transform;
        [SerializeField] private PlayerController _playerController;

        public GameObjectPool EnemyBulletPool => _enemyBulletPool;
        [SerializeField] private GameObjectPool _enemyBulletPool;

        public NavNodeController NavNodeController => _navNodeController;
        [SerializeField] private NavNodeController _navNodeController;

        [SerializeField] private GameObject _restartFrame;

        private void Awake()
        {
            GameEnded.Connect(GameOver);

            IncrementalIDMaker.Clear();

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void GameOver()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            _restartFrame.SetActive(true);
        }

        public void RestartGame()
        {
            GameEnded.DisconnectAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void QuitGame()
        {
            GameEnded.DisconnectAll();
            Application.Quit();
        }
    }
}