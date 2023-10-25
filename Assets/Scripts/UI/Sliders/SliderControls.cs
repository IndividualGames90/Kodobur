using UnityEngine;
using UnityEngine.UI;

namespace IndividualGames.UI
{
    /// <summary>
    /// General slider controls.
    /// </summary>
    public class SliderControls : MonoBehaviour
    {
        [SerializeField] protected Image _slider;

        public virtual void UpdateSliderValue(float value)
        {
            _slider.fillAmount = value;
        }
    }
}