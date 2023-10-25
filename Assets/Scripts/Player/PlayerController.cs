using IndividualGames.CaseLib.DI;
using IndividualGames.CaseLib.Signalization;
using IndividualGames.DI;
using IndividualGames.Enemy;
using IndividualGames.Game;
using IndividualGames.ScriptableObjects;
using IndividualGames.UI;
using System;
using UnityEngine;

namespace IndividualGames.Player
{
    /// <summary>
    /// Root controls for Player. Handles base player game logic and distributes commonalities.
    /// </summary>
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [Header("Data:")]
        [SerializeField] private PlayerStats _playerStats;
        [SerializeField] private GunStats _gunStats;
        [SerializeField] private PlayerLevelingData _levelingData;

        [Header("SubSystem:")]
        [SerializeField] private FPSController _fpsController;
        [SerializeField] private GunController _gunController;

        private PlayerInputs PlayerInputs;

        private BasicSignal<string> _onPlayerHealthUpdate = new();
        private BasicSignal<string> _onEnemyKilledUpdate = new();
        private BasicSignal<string> _onLevelUpUpdate = new();
        private BasicSignal<float> _onHealthSliderChanged = new();
        private BasicSignal<float> _onExperienceSliderChanged = new();

        private PlayerStats _playerStatsPersonal;
        private GunStats _gunStatsPersonal;
        private PlayerLevelingData _levelingDataPersonal;

        private BasicSignal _gameEnded;

        private void Awake()
        {
            PlayerInputs = new();

            _playerStatsPersonal = Instantiate(_playerStats);
            _gunStatsPersonal = Instantiate(_gunStats);
            _levelingDataPersonal = Instantiate(_levelingData);

            _fpsController.Init(new Container(PlayerInputs));
            _gunController.Init(new GunContainerData(PlayerInputs, _gunStatsPersonal));

            SignalizationSetup();
        }

        private void SignalizationSetup()
        {
            var canvasHub = CanvasController.Instance.CanvasHub;

            _onPlayerHealthUpdate = (BasicSignal<string>)canvasHub.AcquireLabelChangeableSignal(LabelPlayerHealth.k_LabelPlayerHealth);
            _onEnemyKilledUpdate = (BasicSignal<string>)canvasHub.AcquireLabelChangeableSignal(PlayerKillCounter.k_PlayerKillCounter);
            _onLevelUpUpdate = (BasicSignal<string>)canvasHub.AcquireLabelChangeableSignal(LevelUpLabel.k_LevelUpLabel);

            _onHealthSliderChanged = (BasicSignal<float>)canvasHub.AcquireLabelChangeableSignal(HealthSliderController.k_HealthSlider);
            _onExperienceSliderChanged = (BasicSignal<float>)canvasHub.AcquireLabelChangeableSignal(ExperienceSliderController.k_ExperienceSlider);

            _onPlayerHealthUpdate.Emit(_playerStatsPersonal.Health.ToString());
            _onEnemyKilledUpdate.Emit(_playerStatsPersonal.KillScore.ToString());
            _onLevelUpUpdate.Emit(_playerStatsPersonal.Level.ToString());

            OnHealthChanged();
            OnExperienceChanged();

            EnemyController.EnemyKilled.Connect(EnemyKilled);
            _gameEnded = GameController.Instance.GameEnded;
        }

        private void OnEnable()
        {
            PlayerInputs.Enable();
        }

        public void Damage(int damage)
        {
            _playerStatsPersonal.Health -= damage;
            _onPlayerHealthUpdate.Emit(_playerStatsPersonal.Health.ToString());
            OnHealthChanged();

            if (_playerStatsPersonal.Health <= 0)
            {
                _gameEnded.Emit();
            }
        }

        /// <summary> Player killed an enemy. </summary>
        public void EnemyKilled(int experienceGained)
        {
            _playerStatsPersonal.KillScore++;
            _onEnemyKilledUpdate.Emit(_playerStatsPersonal.KillScore.ToString());
            ExperienceGained(experienceGained);
        }

        /// <summary> Player leveled up. </summary>
        private void LevelUp()
        {
            _playerStatsPersonal.Level++;
            _onLevelUpUpdate.Emit(_playerStatsPersonal.Level.ToString());
            OnExperienceChanged();
        }

        /// <summary> Gained experience. </summary>
        private void ExperienceGained(int experienceGained)
        {
            _playerStatsPersonal.ExperiencePoints += experienceGained;
            OnExperienceChanged();

            try
            {
                if (_playerStatsPersonal.ExperiencePoints >= _levelingDataPersonal.LevelingGrade[_playerStatsPersonal.Level - 1])
                {
                    _playerStatsPersonal.ExperiencePoints = 0;
                    LevelUp();
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogWarning($"{e.GetType().Name}: Level exceeded.");
            }
        }

        /// <summary> Ammo picked up. </summary>
        /// <returns> null if all ammo consumed, otherwise returns remaining value.</returns>
        public int? AmmoGained(int ammoGained)
        {
            return _gunController.GainAmmo(ammoGained);
        }

        /// <summary> Health picked up. </summary>
        public void HealthGained(int healthGained)
        {
            _playerStatsPersonal.Health += healthGained;
            _playerStatsPersonal.Health = Mathf.Clamp(_playerStatsPersonal.Health, 0, _playerStatsPersonal.HealthMaximum);
            OnHealthChanged();
        }

        private void OnHealthChanged()
        {
            _onHealthSliderChanged.Emit((float)_playerStatsPersonal.Health / _playerStatsPersonal.HealthMaximum);
            _onPlayerHealthUpdate.Emit(_playerStatsPersonal.Health.ToString());
        }

        private void OnExperienceChanged()
        {
            try
            {
                var value = (float)_playerStatsPersonal.ExperiencePoints / _levelingDataPersonal.LevelingGrade[_playerStatsPersonal.Level - 1];
                _onExperienceSliderChanged.Emit(value);
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogWarning($"{e.GetType().Name}: Level exceeded.");
            }
        }

        private void OnDestroy()
        {
            _onPlayerHealthUpdate.DisconnectAll();
            _onEnemyKilledUpdate.DisconnectAll();
            _onLevelUpUpdate.DisconnectAll();
            _onHealthSliderChanged.DisconnectAll();
            _onExperienceSliderChanged.DisconnectAll();
        }
    }
}