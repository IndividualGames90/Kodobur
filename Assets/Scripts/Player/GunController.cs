using IndividualGames.CaseLib.DI;
using IndividualGames.Pool;
using IndividualGames.Weapon;
using System.Collections;
using UnityEngine;

namespace IndividualGames.Player
{
    /// <summary>
    /// Player subsystem that controls the gun firing.
    /// </summary>
    public class GunController : MonoBehaviour, IInitializable
    {
        [SerializeField] private GameObjectPool _bulletPool;
        [SerializeField] private Transform _muzzleTransform;

        private PlayerInputs _input;
        private bool LMBPressed => _input.Player.LMB.ReadValue<float>() > .5f;

        public bool Initialized { get { return _initialized; } set { } }
        private bool _initialized = false;

        private WaitForSeconds _waitGunFireInterval = new(.3f);
        private bool _gunFireLocked = false;

        public void Init(IContainer playerInputs)
        {
            _input = (PlayerInputs)playerInputs.Value;
            _initialized = true;
        }

        private void Update()
        {
            if (!_initialized)
            {
                return;
            }

            if (LMBPressed && !_gunFireLocked)
            {
                StartCoroutine(FireGun());
            }
        }

        /// <summary> Fire gun in intervals. </summary>
        public IEnumerator FireGun()
        {
            _gunFireLocked = true;

            var bullet = _bulletPool.Retrieve();
            bullet.transform.position = _muzzleTransform.position;
            bullet.transform.forward = _muzzleTransform.forward;
            bullet.GetComponent<BulletController>().Fired(true, _bulletPool);
            yield return _waitGunFireInterval;

            _gunFireLocked = false;
        }
    }
}