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
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Player))
            {
                other.GetComponent<PlayerController>().Damage(10);
            }
        }
    }
}

