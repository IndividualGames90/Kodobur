using IndividualGames.CaseLib.DI;
using IndividualGames.CaseLib.Signalization;
using IndividualGames.DI;
using IndividualGames.ScriptableObjects;
using IndividualGames.UI;
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

        private BasicSignal<string> _onPlayerHealthUpdate = new();
        private BasicSignal<string> _onEnemyKilledUpdate = new();

        private void Awake()
        {
            PlayerInputs = new();
            _playerStats.Health = _playerStats.HealthMaximum * _playerStats.Level;
            _fpsController.Init(new Container(PlayerInputs));
            _gunController.Init(new GunContainerData(PlayerInputs, _gunStats));

            SignalizationSetup();
        }

        private void SignalizationSetup()
        {
            var canvasHub = CanvasController.Instance.CanvasHub;

            _onPlayerHealthUpdate = (BasicSignal<string>)canvasHub.AcquireLabelChangeableSignal(LabelPlayerHealth.k_LabelPlayerHealth);
            _onEnemyKilledUpdate = (BasicSignal<string>)canvasHub.AcquireLabelChangeableSignal(PlayerKillCounter.k_PlayerKillCounter);

            _onPlayerHealthUpdate.Emit(_playerStats.Health.ToString());
            _onEnemyKilledUpdate.Emit(_playerStats.KillScore.ToString());
        }

        private void OnEnable()
        {
            PlayerInputs.Enable();
        }

        public void Damage(int damage)
        {
            _playerStats.Health -= damage;
            _onPlayerHealthUpdate.Emit(_playerStats.Health.ToString());

            if (_playerStats.Health >= 0)
            {
                PlayerKilled.Emit();
            }
        }

        /// <summary> Player killed an enemy. </summary>
        public void EnemyKilled()
        {
            _playerStats.KillScore++;
            _onEnemyKilledUpdate.Emit(_playerStats.KillScore.ToString());
        }

        /// <summary> Player leveled up. </summary>
        private void LevelUp()
        {
            _playerStats.Level++;
        }
    }
}