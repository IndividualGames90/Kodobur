using UnityEngine;
using UnityEngine.UI;

namespace IndividualGames.UI
{
    /// <summary>
    /// Specific Control for 3D enemy health.
    /// </summary>
    public class EnemyHealthSlider : MonoBehaviour
    {
        [SerializeField] private Image _slider;
        [SerializeField] private GameObject _bg;

        public void UpdateSliderValue(float value)
        {
            _slider.fillAmount = value;

            if (value == 0)
            {
                _bg.SetActive(false);
            }
        }
    }
}