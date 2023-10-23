using UnityEngine;

namespace IndividualGames.Unity
{
    /// <summary>
    /// Unity Layers
    /// </summary>
    public static class Layers
    {
        public static readonly int Player = LayerMask.NameToLayer("Player");
        public static readonly int Ground = LayerMask.NameToLayer("Environment");
    }
}