using IndividualGames.CaseLib.DataStructures;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IndividualGames.DS
{
    /// <summary>
    /// Singleton factory for game objects.
    /// </summary>
    public class GameObjectFactory : SingletonBehavior<GameObjectFactory>
    {
        [SerializeField] private GameObject[] _prefabs;

        private Dictionary<int, GameObject> _products = new();

        private void Awake()
        {
            foreach (var prefab in _prefabs)
            {
                _products.Add(prefab.name.GetHashCode(), prefab);
            }
        }

        public GameObject CreateObjectOfName(GameObject go)
        {
            if (_products.TryGetValue(go.name.GetHashCode(), out GameObject value))
            {
                return Instantiate(value);
            }
            else
            {
                throw new ArgumentException($"{nameof(GameObjectFactory)}: Value not found for {go}");
            }
        }
    }
}