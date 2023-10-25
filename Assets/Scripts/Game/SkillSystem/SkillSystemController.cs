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

        /// <summary> Callback for any button click from skill tab. </summary>
        private void OnButtonClicked(GameObject pressedButtonObject)
        {
            var fieldInfo = typeof(SkillUpgrades).GetField(pressedButtonObject.name,
                                                           System.Reflection.BindingFlags.Public |
                                                           System.Reflection.BindingFlags.Instance);


            var checkResultPair = CheckIfEnoughPoints(fieldInfo);
            if (checkResultPair.Item1)
            {
                _skillUpgradesPersonal.SkillPoints -= checkResultPair.Item2;
                UpdatePointsView();

                var fieldValue = fieldInfo.GetValue(_skillUpgradesPersonal);


                //TODO: Upgrade related skill.
            }
            else
            {
                _skillTabView.FlashNotEnoughPointsLabel();
            }
        }

        /// <summary> Check if have enought points to upgrade. </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        private (bool, int) CheckIfEnoughPoints(FieldInfo fieldInfo)
        {
            if (fieldInfo.FieldType == typeof(bool))
            {
                bool notBelowZero = _skillUpgradesPersonal.SkillPoints - _skillUpgradesPersonal.RequiredPointsForBool >= 0;
                return (_skillUpgradesPersonal.SkillPoints >= _skillUpgradesPersonal.RequiredPointsForBool
                        && notBelowZero,
                        _skillUpgradesPersonal.RequiredPointsForBool);
            }
            else
            {
                bool notBelowZero = _skillUpgradesPersonal.SkillPoints - _skillUpgradesPersonal.RequiredPointsForBool >= 0;
                return (_skillUpgradesPersonal.SkillPoints >= _skillUpgradesPersonal.RequiredPointsForInt
                        && notBelowZero,
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

        private void UpdatePointsView()
        {
            _skillTabView.UpdateRemainingPointsCounter(_skillUpgradesPersonal.SkillPoints);
        }
    }
}