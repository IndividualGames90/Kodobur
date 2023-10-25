using IndividualGames.DS;
using IndividualGames.Enemy;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace IndividualGames.Game
{
    /// <summary>
    /// Spawner for enemies.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private GameObject _locationsParent;
        [SerializeField] private int _maxEnemyCount = 5;

        private Transform[] _locations;
        private List<int> _spawnedLocationIDs = new();

        private Stopwatch _timer = new();
        private int _spawningInterval = 3;
        private int _currentEnemyCount;

        private void Awake()
        {
            _locations = GetComponentsInChildren<Transform>();
            _timer.Start();

            EnemyController.EnemyKilled.Connect(OnEnemyDestroyed);
        }

        private void FixedUpdate()
        {
            if (_timer.Elapsed.TotalSeconds < _spawningInterval)
            {
                return;
            }

            if (_currentEnemyCount < _maxEnemyCount)
            {
                _timer.Restart();
                var go = GameObjectFactory.Instance.CreateObjectOfName(_prefab);
                go.transform.position = RandomSpawnLocation();
                go.name += _currentEnemyCount.ToString();

                _currentEnemyCount++;
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

        /// <summary> An enemy is destroyed. </summary>
        private void OnEnemyDestroyed(int experinceValue)
        {
            _currentEnemyCount--;
        }
    }
}