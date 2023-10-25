using IndividualGames.Player;
using IndividualGames.ScriptableObjects;
using IndividualGames.UI;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace IndividualGames.Game
{
    /// <summary>
    /// Controller for skills system.
    /// </summary>
    public class SkillSystemController : MonoBehaviour
    {
        [SerializeField] private GameObject _skillTab;
        [SerializeField] private SkillUpgrades _skillUpgrades;

        /// <summary> Read TAB input from player. </summary>
        private PlayerController _playerController;

        private WaitForSeconds _waitToggle = new(.2f);
        private bool _toggleLocked = false;

        private SkillUpgrades _skillUpgradesPersonal;
        private SkillTabView _skillTabView;

        private void Awake()
        {
            _playerController = GameController.Instance.PlayerLocation.transform.GetComponent<PlayerController>();
            _playerController.PlayerLevelUp.Connect(OnPlayerLevelUp);

            _skillUpgradesPersonal = Instantiate(_skillUpgrades);

            _skillTabView = _skillTab.GetComponent<SkillTabView>();
            _skillTabView.PressedButtonGameObject.Connect(OnButtonClicked);

            UpdatePointsView();
        }

        private void Update()
        {
            if (_playerController == null)
            {
                return;
            }

            if (_playerController.TABKey && !_toggleLocked)
            {
                StartCoroutine(ToggleSkillTab());
            }
        }

        /// <summary> Callback for skill tab button clicks. </summary>
        private void OnButtonClicked(GameObject pressedButtonObject)
        {
            var fieldInfo = typeof(SkillUpgrades).GetField(pressedButtonObject.name,
                                                           System.Reflection.BindingFlags.Public |
                                                           System.Reflection.BindingFlags.Instance);


            var checkResultPair = CheckIfEnoughPoints(fieldInfo);
            if (checkResultPair.Item1)
            {
                var fieldValue = fieldInfo.GetValue(_skillUpgradesPersonal);

                if (fieldInfo.FieldType == typeof(int) && (int)fieldValue < 5)
                {
                    fieldInfo.SetValue(_skillUpgradesPersonal, (int)fieldValue + 1);
                    _skillUpgradesPersonal.SkillPoints -= checkResultPair.Item2;
                }
                else if (fieldInfo.FieldType == typeof(bool) && !(bool)fieldValue)
                {
                    fieldInfo.SetValue(_skillUpgradesPersonal, true);
                    _skillUpgradesPersonal.SkillPoints -= checkResultPair.Item2;

                }
            }
            else
            {
                _skillTabView.FlashNotEnoughPointsLabel();
            }

            UpdatePointsView();
        }

        /// <summary> Check if have enought points to upgrade. </summary>
        /// <returns>Check result bool and amount of points to deduce.</returns>
        private (bool, int) CheckIfEnoughPoints(FieldInfo fieldInfo)
        {
            if (fieldInfo.FieldType == typeof(bool))
            {
                bool notBelowZero = _skillUpgradesPersonal.SkillPoints - _skillUpgradesPersonal.RequiredPointsForBool >= 0;
                bool hasEnoughPoints = _skillUpgradesPersonal.SkillPoints >= _skillUpgradesPersonal.RequiredPointsForBool;

                return (hasEnoughPoints && notBelowZero,
                        _skillUpgradesPersonal.RequiredPointsForBool);
            }
            else
            {
                bool notBelowZero = _skillUpgradesPersonal.SkillPoints - _skillUpgradesPersonal.RequiredPointsForInt >= 0;
                bool hasEnoughPoints = _skillUpgradesPersonal.SkillPoints >= _skillUpgradesPersonal.RequiredPointsForInt;

                return (hasEnoughPoints && notBelowZero,
                        _skillUpgradesPersonal.RequiredPointsForInt);
            }
        }

        /// <summary> Gain a skill point on player level up. </summary>
        private void OnPlayerLevelUp()
        {
            _skillUpgradesPersonal.SkillPoints++;
            UpdatePointsView();
        }

        /// <summary> Show/Hide skill tab. </summary>
        public IEnumerator ToggleSkillTab()
        {
            _toggleLocked = true;

            _skillTab.SetActive(!_skillTab.activeSelf);

            Cursor.visible = _skillTab.activeSelf;
            Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;

            yield return _waitToggle;

            _toggleLocked = false;
        }

        /// <summary> Update the points display in the skill tab.</summary>
        private void UpdatePointsView()
        {
            _skillTabView.UpdateRemainingPointsCounter(_skillUpgradesPersonal.SkillPoints);

            _playerController.UpdateSkills(_skillUpgradesPersonal);
        }
    }
}