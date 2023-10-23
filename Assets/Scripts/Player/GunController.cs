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
            if (!_initialized)
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
            return _gunStats.CurrentAmmo != 0;
        }

        /// <summary> Fire gun in intervals. </summary>
        private IEnumerator FireGun()
        {
            _gunFireLocked = true;

            SpendAmmo();

            var bullet = _bulletPool.Retrieve();
            bullet.transform.position = _muzzleTransform.position;
            bullet.transform.forward = _muzzleTransform.forward;
            bullet.GetComponent<BulletController>().Fired(_gunStats.AttackDamage, true, _bulletPool);

            yield return _waitGunFireInterval;

            _gunFireLocked = false;
        }

        private void OnAmmoUpdate()
        {
            _onAmmoUpdate.Emit(_gunStats.CurrentAmmo.ToString());
        }

        private void SpendAmmo()
        {
            _gunStats.CurrentAmmo--;
            OnAmmoUpdate();
        }

        private void GainAmmo()
        {
            _gunStats.CurrentAmmo++;
            OnAmmoUpdate();
        }
    }
}