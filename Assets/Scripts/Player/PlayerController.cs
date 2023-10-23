using IndividualGames.CaseLib.DI;
using IndividualGames.CaseLib.Signalization;
using IndividualGames.DI;
using IndividualGames.ScriptableObjects;
using UnityEngine;

namespace IndividualGames.Player
{
    /// <summary>
    /// Root controls for Player. Handles base player game logic and distributes commonalities.
    /// </summary>
    public class PlayerController : MonoBehaviour, IDamageable
    {
        public readonly BasicSignal PlayerKilled = new();

        [Header("Data:")]
        [SerializeField] private PlayerStats _playerStats;
        [SerializeField] private GunStats _gunStats;

        [Header("SubSystem:")]
        [SerializeField] private FPSController _fpsController;
        [SerializeField] private GunController _gunController;

        private PlayerInputs PlayerInputs;

        private void Awake()
        {
            PlayerInputs = new();
            _playerStats.Health = _playerStats.HealthMaximum * _playerStats.Level;
            _fpsController.Init(new Container(PlayerInputs));
            _gunController.Init(new Container(PlayerInputs));
        }

        private void OnEnable()
        {
            PlayerInputs.Enable();
        }

        public void Damage(int damage)
        {
            _playerStats.Health -= damage;

            if (_playerStats.Health >= 0)
            {
                PlayerKilled.Emit();
            }
        }

        /// <summary> Player killed an enemy. </summary>
        public void EnemyKilled()
        {
            _playerStats.KillScore++;
        }

        /// <summary> Player leveled up. </summary>
        private void LevelUp()
        {
            _playerStats.Level++;
        }
    }
}