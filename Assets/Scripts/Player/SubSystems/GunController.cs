using IndividualGames.CaseLib.DI;
using IndividualGames.CaseLib.Signalization;
using IndividualGames.Pool;
using IndividualGames.ScriptableObjects;
using IndividualGames.UI;
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
        private bool _sprinting => _input.Player.Sprint.ReadValue<float>() > .5f;

        private GunStats _gunStats;

        public bool Initialized { get { return _initialized; } set { } }
        private bool _initialized = false;

        private WaitForSeconds _waitGunFireInterval = new(.3f);
        private bool _gunFireLocked = false;

        private BasicSignal<string> _onAmmoUpdate = new();

        public void Init(GunContainerData gunData)
        {
            _input = gunData.PlayerInputs;
            _gunStats = gunData.GunStats;
            _initialized = true;

            SignalizationSetup();
        }

        private void SignalizationSetup()
        {
            var canvasHub = CanvasController.Instance.CanvasHub;
            _onAmmoUpdate = (BasicSignal<string>)canvasHub.AcquireLabelChangeableSignal(PlayerAmmoCounter.k_PlayerAmmoCounter);
            OnAmmoUpdate();
        }

        private void Update()
        {
            if (!_initialized || _sprinting)
            {
                return;
            }

            if (LMBPressed && !_gunFireLocked && HasBullets())
            {
                StartCoroutine(FireGun());
            }
        }

        /// <summary> Check if has bullets to fire. </summary>
        private bool HasBullets()
        {
            return _gunStats.BulletCarried != 0;
        }

        /// <summary> Fire gun in intervals. </summary>
        private IEnumerator FireGun()
        {
            _gunFireLocked = true;

            SpendAmmo();

            int bulletCount = _gunStats.TripleShot ? 3 : 1;
            int[] forwardDegrees = new int[] { 0, -15, 15 };
            for (int i = 0; i < bulletCount; i++)
            {
                var bullet = _bulletPool.Retrieve();
                bullet.GetComponent<BulletController>().Fired(_gunStats.AttackDamage, true, _bulletPool, _gunStats.PierceShot);
                bullet.transform.position = _muzzleTransform.position;
                bullet.transform.forward = Quaternion.Euler(0, forwardDegrees[i], 0) * _muzzleTransform.forward;
            }

            yield return _waitGunFireInterval;

            _gunFireLocked = false;
        }

        /// <summary> Change happened on ammo. </summary>
        private void OnAmmoUpdate()
        {
            _onAmmoUpdate.Emit(_gunStats.BulletCarried.ToString());
        }

        /// <summary> Single ammo is spent. </summary>
        private void SpendAmmo()
        {
            _gunStats.BulletCarried--;
            OnAmmoUpdate();
        }

        /// <summary> Ammo picked up. </summary>
        /// returns null if all ammo consumed, otherwise returns remaining value.
        public int? GainAmmo(int ammoGained)
        {
            if (_gunStats.BulletCarried == _gunStats.BulletCapacity)
            {
                return ammoGained;
            }

            var haveRoomForAmmo = _gunStats.BulletCapacity - _gunStats.BulletCarried >= ammoGained;
            if (haveRoomForAmmo)
            {
                _gunStats.BulletCarried += ammoGained;
                OnAmmoUpdate();
                return null;
            }
            else
            {
                var exceedingBulletCount = _gunStats.BulletCarried + ammoGained;
                _gunStats.BulletCarried = _gunStats.BulletCapacity;
                OnAmmoUpdate();
                return exceedingBulletCount - _gunStats.BulletCapacity;
            }
        }

        private void OnDestroy()
        {
            _onAmmoUpdate.DisconnectAll();
        }
    }
}