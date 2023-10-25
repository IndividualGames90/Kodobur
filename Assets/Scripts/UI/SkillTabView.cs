using IndividualGames.CaseLib.Signalization;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IndividualGames.UI
{
    /// <summary>
    /// View for skill tab frame.
    /// </summary>
    public class SkillTabView : MonoBehaviour
    {
        [SerializeField] private GameObject[] _buttonGameObjects;
        [SerializeField] private TextMeshProUGUI _remainingPointsCounter;
        [SerializeField] private GameObject _notEnoughPointsLabel;

        public BasicSignal<GameObject> PressedButtonGameObject = new();

        private WaitForSeconds _flashWait = new(1.5f);

        private void Awake()
        {
            foreach (var button in _buttonGameObjects)
            {
                button.GetComponent<Button>().onClick.AddListener(() => { ButtonPressed(button); });
            }
        }

        /// <summary> Update remaining points. </summary>
        public void UpdateRemainingPointsCounter(int value)
        {
            _remainingPointsCounter.text = value.ToString();
        }

        /// <summary> Flash label for a brief second. </summary>
        public void FlashNotEnoughPointsLabel()
        {
            StopAllCoroutines();
            StartCoroutine(FlashLabel());
        }

        private IEnumerator FlashLabel()
        {
            _notEnoughPointsLabel.SetActive(true);
            yield return _flashWait;
            _notEnoughPointsLabel.SetActive(false);
        }

        /// <summary> Emit the button pressed for consumption. </summary>
        private void ButtonPressed(GameObject pressedButtonObject)
        {
            PressedButtonGameObject.Emit(pressedButtonObject);
        }
    }
}