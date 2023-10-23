using IndividualGames.ItemDrops;
using IndividualGames.Unity;
using UnityEngine;

namespace IndividualGames.Player
{
    /// <summary>
    /// Controls pickups for player.
    /// </summary>
    public class PickUpController : MonoBehaviour
    {
        [SerializeField] PlayerController _controller;

        private void OnTriggerEnter(Collider other)
        {
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
                    Destroy(other.gameObject);
                }
            }
            else if (other.CompareTag(Tags.Health))
            {
                _controller.HealthGained(other.GetComponent<ValueContainer>().Value);
                Destroy(other.gameObject);
            }
        }
    }
}
