using IndividualGames.Player;
using IndividualGames.ScriptableObjects;
using IndividualGames.UI;
using System.Collections;
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

        private void Awake()
        {
            _playerController = GameController.Instance.PlayerLocation.transform.GetComponent<PlayerController>();
            _playerController.PlayerLevelUp.Connect(OnPlayerLevelUp);

            _skillUpgradesPersonal = Instantiate(_skillUpgrades);

            _skillTab.GetComponent<SkillTabView>().PressedButtonGameObject.Connect(ButtonClicked);
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

        private void ButtonClicked(GameObject pressedButtonObject)
        {
            if (CheckIfEnoughPoints())
            {
                _skillUpgradesPersonal.SkillPoints--;
                //TODO: Upgrade related skill.
            }
        }

        private void OnPlayerLevelUp()
        {
            _skillUpgradesPersonal.SkillPoints++;
        }

        private bool CheckIfEnoughPoints()
        {
            //TODO : Really check enough points for that skill.
            return false;
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
    }
}