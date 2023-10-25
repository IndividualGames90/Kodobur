using UnityEngine;
using UnityEngine.UI;

namespace IndividualGames.UI
{
    /// <summary>
    /// General slider controls.
    /// </summary>
    public class SliderControls : MonoBehaviour
    {
        [SerializeField] private bool _startZero;
        [SerializeField] protected Image _slider;

        private void Awake()
        {
            _slider.fillAmount = _startZero ? 0 : 1;
        }

        public virtual void UpdateSliderValue(float value)
        {
            _slider.fillAmount = value;
        }
    }
}