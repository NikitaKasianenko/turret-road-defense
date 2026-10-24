using Game.Level;
using UnityEngine;

namespace Game.Weapons
{
    public class Turret : MonoBehaviour, IResettable
    {
        [SerializeField] private TurretAim _aim;
        [SerializeField] private TurretGun _gun;
        [SerializeField] private AimLine _aimLine;

        private void Awake() =>
            Deactivate();

        public void Activate() =>
            SetActive(true);

        public void Deactivate() =>
            SetActive(false);

        public void ResetState() =>
            _aim.ResetAim();

        private void SetActive(bool isActive)
        {
            _aim.enabled = isActive;
            _gun.enabled = isActive;
            _aimLine.gameObject.SetActive(isActive);
        }
    }
}
