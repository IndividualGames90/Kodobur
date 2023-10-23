using IndividualGames.CaseLib.DataStructures;
using IndividualGames.CaseLib.UI;
using UnityEngine;

namespace IndividualGames.UI
{
    /// <summary>
    /// Canvas singleton for access.
    /// </summary>
    public class CanvasController : SingletonBehavior<CanvasController>
    {
        public CanvasHub CanvasHub => _canvasHub;
        [SerializeField] private CanvasHub _canvasHub;
    }
}