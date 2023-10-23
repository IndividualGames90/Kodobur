using UnityEngine;

namespace IndividualGames.Unity
{
    /// <summary>
    /// Unity Layers
    /// </summary>
    public static class Layers
    {
        public static readonly int Player = 1 << LayerMask.NameToLayer("Player");
        public static readonly int Ground = 1 << LayerMask.NameToLayer("Environment");
    }
}