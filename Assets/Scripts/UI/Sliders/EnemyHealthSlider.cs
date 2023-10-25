using UnityEngine;

namespace IndividualGames.UI
{
    /// <summary>
    /// Specific Control for 3D enemy health.
    /// </summary>
    public class EnemyHealthSlider : Slider
    {
        [SerializeField] private GameObject _backgroundImage;

        public override void UpdateSliderValue(float value)
        {
            _slider.fillAmount = value;

            if (value == 0)
            {
                _backgroundImage.SetActive(false);
            }
        }
    }
}