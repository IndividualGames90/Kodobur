using IndividualGames.CaseLib.Signalization;
using TMPro;
using UnityEngine;

namespace IndividualGames.ItemDrops
{
    /// <summary>
    /// Generic container of a value. Retains and serves the value to others.
    /// </summary>
    public class ValueContainer : MonoBehaviour
    {
        public readonly BasicSignal Destroyed = new();

        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                _valueLabel.text = value.ToString();
            }
        }
        [SerializeField] private int _value = 0;

        [SerializeField] private TextMeshPro _valueLabel;

        private void Awake()
        {
            Value = _value;
        }

        public void OnDestroyed()
        {
            Destroyed.Emit();
            Destroy(gameObject);
        }
    }
}