using IndividualGames.Player;
using IndividualGames.Unity;
using UnityEngine;

namespace IndividualGames.Other
{
    /// <summary>
    /// Gives 10 damage when entered.
    /// </summary>
    public class DamageZone : MonoBehaviour
    {
        [SerializeField] private int _damage = 10;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Player))
            {
                other.GetComponent<PlayerController>().Damage(_damage);
            }
        }
    }
}

