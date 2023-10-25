using UnityEngine;
using UnityEngine.UI;

namespace IndividualGames.UI
{
    /// <summary>
    /// General slider.
    /// </summary>
    public class Slider : MonoBehaviour
    {
        [SerializeField] protected Image _slider;

        public virtual void UpdateSliderValue(float value)
        {
            _slider.fillAmount = value;
        }
    }
}