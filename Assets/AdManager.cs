using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gley.MobileAds;
using System;


public class AdManager : MonoBehaviour
{


    private void Start()
    {
#if PLATFORM_ANDROID
        APIMobileAds.Initialize(OnInitialized);
#endif

    }

    private void OnInitialized()
    {
        APIMobileAds.ShowBanner(BannerPosition.Bottom, BannerType.Adaptive);
        APIMobileAds.ShowBuiltInConsentPopup(() => { });
        if (!APIMobileAds.GDPRConsentWasSet())
        {
            APIMobileAds.ShowBuiltInConsentPopup(PopupCloseds);
        }
    }

    private void PopupCloseds()
    {

    }


}
