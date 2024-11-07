using System;
using UnityEngine;

using MoreMountains.NiceVibrations;

namespace Fishtail.PlayTheBall.Vibration
{
    public class VibrationController : MonoBehaviour
    {
        public static VibrationController instance { get; private set; }

        public bool vibrate;

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
        public void ImpactFailure()
        {
            if (!vibrate) { return; }
            print("Shake");
            MMVibrationManager.Haptic(HapticTypes.Failure);
        }
        public void ImpactSuccess()
        {
            if (!vibrate) { return; }
            MMVibrationManager.Haptic(HapticTypes.Success);
        }
        public void ImpactWarning()
        {
            if (!vibrate) { return; }
            MMVibrationManager.Haptic(HapticTypes.Warning);
        }
    }
}
