using IndividualGames.DS;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace IndividualGames.ItemDrops
{
    /// <summary>
    /// Ammo spawner.
    /// </summary>
    public class DropSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private GameObject _locationsParent;
        [SerializeField] private int _maxDropCount = 3;

        private Transform[] _locations;
        private List<int> _spawnedLocationIDs = new();

        private Stopwatch _timer = new();
        private int _spawningInterval = 3;
        private int _currentDropCount;

        private void Awake()
        {
            _locations = GetComponentsInChildren<Transform>();
            _timer.Start();
        }

        private void FixedUpdate()
        {
            if (_timer.Elapsed.TotalSeconds < _spawningInterval)
            {
                return;
            }

            if (_currentDropCount < _maxDropCount)
            {
                _timer.Restart();
                var go = GameObjectFactory.Instance.CreateObjectOfName(_prefab);
                go.transform.position = RandomSpawnLocation();
                go.GetComponent<ValueContainer>().Destroyed.Connect(OnDropDestroyed);

                _currentDropCount++;
            }
        }

        /// <summary> Get a random position to spawn. </summary>
        private Vector3 RandomSpawnLocation()
        {
            var index = Random.Range(0, _locations.Length);

            while (_spawnedLocationIDs.Contains(index))
            {
                index = Random.Range(0, _locations.Length);
            }
            _spawnedLocationIDs.Add(index);

            if (_spawnedLocationIDs.Count > 5)
            {
                _spawnedLocationIDs.Clear();
            }

            return _locations[index].position;
        }

        /// <summary> A drop is destroyed. </summary>
        private void OnDropDestroyed()
        {
            _timer.Restart();
            _currentDropCount--;
        }
    }
}