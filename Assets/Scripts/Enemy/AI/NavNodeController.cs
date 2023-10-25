using IndividualGames.CaseLib.DataStructures;
using System.Collections.Generic;
using UnityEngine;

namespace IndividualGames.Enemy
{
    /// <summary>
    /// Holder and passer on navnode transforms.
    /// </summary>
    public class NavNodeController : SingletonBehavior<NavNodeController>
    {
        private List<Transform> _navNodes = new();

        private void Awake()
        {
            var transforms = GetComponentsInChildren<Transform>();

            foreach (var t in transforms)
            {
                _navNodes.Add(t);
            }
        }

        /// <summary> Get a random navnode position. </summary>
        public Vector3 AcquireRandomNavNode()
        {
            return _navNodes[Random.Range(0, _navNodes.Count - 1)].position;
        }
    }
}