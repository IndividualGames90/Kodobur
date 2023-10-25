using IndividualGames.CaseLib.Signalization;
using IndividualGames.CaseLib.Utils;
using UnityEngine;

namespace IndividualGames.Player
{
    /// <summary>
    /// Checks is grounded on Environment layer.
    /// </summary>
    public class GroundedChecker : MonoBehaviour
    {
        [SerializeField] private float _groundedDistance = .1f;

        public readonly BasicSignal<bool> Grounded = new();

        private void Update()
        {
            var grounded = Raycaster.HitGround(transform.position, _groundedDistance).Item1;
            Grounded.Emit(grounded);
        }
    }
}