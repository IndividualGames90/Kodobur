using IndividualGames.CaseLib.Signalization;
using UnityEngine;

namespace IndividualGames.Unity
{
    /// <summary>
    /// Trigger emitted for player collisions.
    /// </summary>
    public class TriggerWithPlayer : MonoBehaviour
    {
        [SerializeField] private bool _destroyedOnTrigger = false;

        public readonly BasicSignal TriggeredByPlayer = new();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Player))
            {
                TriggeredByPlayer.Emit();

                if (_destroyedOnTrigger)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}