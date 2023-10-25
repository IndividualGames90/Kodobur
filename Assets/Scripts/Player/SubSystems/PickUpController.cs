using IndividualGames.ItemDrops;
using IndividualGames.Unity;
using System.Collections;
using UnityEngine;

namespace IndividualGames.Player
{
    /// <summary>
    /// Controls pickups for player.
    /// </summary>
    public class PickUpController : MonoBehaviour
    {
        [SerializeField] PlayerController _controller;

        private WaitForSeconds _waitDelay = new(.2f);

        private bool _triggerLocked = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!_triggerLocked)
            {
                StartCoroutine(TriggerDelay(other));
            }
        }

        private IEnumerator TriggerDelay(Collider other)
        {
            _triggerLocked = true;

            if (other.CompareTag(Tags.Ammo))
            {
                var valueContainer = other.GetComponent<ValueContainer>();
                var remainingBulletCount = _controller.AmmoGained(valueContainer.Value);
                if (remainingBulletCount != null)
                {
                    valueContainer.Value = (int)remainingBulletCount;
                }
                else
                {
                    valueContainer.OnDestroyed();
                }
            }
            else if (other.CompareTag(Tags.Health))
            {
                var valueContainer = other.GetComponent<ValueContainer>();
                _controller.HealthGained(valueContainer.Value);
                valueContainer.OnDestroyed();
            }

            yield return _waitDelay;

            _triggerLocked = false;
        }
    }
}
