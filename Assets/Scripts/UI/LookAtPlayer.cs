using IndividualGames.Game;
using UnityEngine;

namespace IndividualGames.UI
{
    /// <summary>
    /// This object will look at player without Y rotation.
    /// </summary>
    public class LookAtPlayer : MonoBehaviour
    {
        [SerializeField] private bool _reverseRotation = false;
        private Transform _playerLocation;

        private void Awake()
        {
            _playerLocation = GameController.Instance.PlayerLocation;
        }

        private void FixedUpdate()
        {
            Vector3 directionToPlayer = _playerLocation.position - transform.position;
            directionToPlayer.y = 0;
            Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = _reverseRotation ? (rotation * Quaternion.Euler(0, 180, 0)) : rotation;
        }
    }
}