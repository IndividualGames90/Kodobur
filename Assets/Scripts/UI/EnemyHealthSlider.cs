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
        [SerializeField] private GameObject _backgroundImage;

        public void UpdateSliderValue(float value)
        {
            _slider.fillAmount = value;

            if (value == 0)
            {
                _backgroundImage.SetActive(false);
            }
        }
    }
}