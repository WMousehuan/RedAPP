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
        API.ShowBanner(BannerPosition.Bottom, BannerType.Adaptive);

        if (!API.GDPRConsentWasSet())
        {
            API.ShowBuiltInConsentPopup(PopupCloseds);
        }
    }

    private void PopupCloseds()
    {

    }


}
