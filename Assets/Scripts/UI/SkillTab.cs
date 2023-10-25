using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTab : MonoBehaviour
{
    [SerializeField] private GameObject[] _buttonGameObjects;
    [SerializeField] private TextMeshProUGUI _remainingPointsCounter;
    [SerializeField] private GameObject _notEnoughPointsLabel;

    private Dictionary<int, GameObject> _buttons;
    private WaitForSeconds _flashWait = new(1.5f);

    private void Awake()
    {
        foreach (var button in _buttonGameObjects)
        {
            if (!_buttons.TryAdd(button.name.GetHashCode(), button))
            {
                Debug.Log($"SkillTab: Button failed to add was {button.name}");
            }
        }
    }

    private void CheckEnoughPoints()
    {

    }

    private void UpdateRemainingPointsCounter(int value)
    {
        _remainingPointsCounter.text = value.ToString();
    }

    private void FlashNotEnoughPointsLabel()
    {
        StartCoroutine(FlashLabel());
    }

    private IEnumerator FlashLabel()
    {
        _notEnoughPointsLabel.SetActive(true);
        yield return _flashWait;
        _notEnoughPointsLabel.SetActive(false);
    }
}
