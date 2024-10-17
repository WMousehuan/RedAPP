using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gley.MobileAds;
using System;


public class AdManager : MonoBehaviour
{


    private void Start()
    {
       // API.Initialize(OnInitialized);

    }

    private void OnInitialized()
    {
        APIMobileAds.ShowBanner(BannerPosition.Bottom, BannerType.Adaptive);

        if (!APIMobileAds.GDPRConsentWasSet())
        {
            APIMobileAds.ShowBuiltInConsentPopup(PopupCloseds);
        }
    }

    private void PopupCloseds()
    {

    }


}
