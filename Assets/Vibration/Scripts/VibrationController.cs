using System;
using UnityEngine;

using MoreMountains.NiceVibrations;

namespace Fishtail.PlayTheBall.Vibration
{
    public class VibrationController : MonoBehaviour
    {
        public static VibrationController instance { get; private set; }

        public bool vibrate { get; set; }

        private void Awake()
        {
            instance = this;

            MMVibrationManager.iOSInitializeHaptics();
        }

        private void OnDestroy()
        {
			MMVibrationManager.iOSReleaseHaptics();
        }

        public void ImpactLight()
        {
            if (!vibrate) { return; }
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }
        public void ImpactHeavy()
        {
            if (!vibrate) { return; }
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
        }
        public void ImpactMedium()
        {
            if (!vibrate) { return; }
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        }
    }
}
