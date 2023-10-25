using IndividualGames.Player;
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

        /// <summary> Read TAB input from player. </summary>
        private PlayerController _playerController;
        private WaitForSeconds _waitToggle = new(.2f);
        private bool _toggleLocked = false;

        private void Awake()
        {
            _playerController = GameController.Instance.PlayerLocation.transform.GetComponent<PlayerController>();
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

        /// <summary> Show/Hide skill tab. </summary>
        public IEnumerator ToggleSkillTab()
        {
            _toggleLocked = true;

            _skillTab.SetActive(!_skillTab.activeSelf);
            yield return _waitToggle;

            _toggleLocked = false;
        }
    }
}