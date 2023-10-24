using IndividualGames.Unity;
using System.Diagnostics;
using UnityEngine;

namespace IndividualGames.Player
{
    /// <summary>
    /// Laser sight for gun
    /// </summary>
    public class LaserSight : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Transform _muzzle;

        Stopwatch _timer = new();
        private bool _updateLocked = false;
        private const float _updateInterval = 0.001f;

        private RaycastHit _hit;

        private void Awake()
        {
            _timer.Start();

            _lineRenderer.SetPosition(0, _muzzle.position);
        }

        private void Update()
        {
            if (_timer.Elapsed.TotalSeconds < _updateInterval)
            {
                return;
            }

            if (!_updateLocked)
            {
                _updateLocked = true;

                UpdateLaserSight();

                _timer.Restart();
                _updateLocked = false;
            }
        }

        private void UpdateLaserSight()
        {
            if (Physics.Raycast(_muzzle.position,
                                _muzzle.forward,
                                out _hit,
                                Mathf.Infinity,
                                Layers.Ground | Layers.Enemy))
            {
                _lineRenderer.SetPosition(0, _muzzle.position);
                _lineRenderer.SetPosition(1, _hit.point);
            }
        }
    }
}