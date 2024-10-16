/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gley.EasyIAP;
using EasyUI.Toast;
using Newtonsoft.Json;
using Gley.DailyRewards.Internal;
using static UnityEngine.GraphicsBuffer;
using System.Reflection;


public class PopupShopCoin : Popup
{
    public enum GoodsType
    {

    }

    public class DM_StoreProducts
    {
        public ShopProductNames name;
        public bool bought;

        public DM_StoreProducts(ShopProductNames name, bool bought)
        {
            this.name = name;
            this.bought = bought;
        }
    }

    private string getCoinShopDataUrl = "/app-api/red/recharge-award/page";


    private bool purchaseInProgress;
    private bool initializationInProgress;
    private List<DM_StoreProducts> consumableProducts = new List<DM_StoreProducts>();

    public static bool isNewUser;

    public UipopupTreasureShopApi uipopupTreasureShopApi;

    public GameObject goods_Prefab;

    public ObjectGroup<string, Sprite> sprite_Group;

	public Button WatchAds;

	public Image ImageBackBlocking;

    public GridLoopScroll_Ctrl loopScroll_Ctrl;

    public List<CoinShopDataVO> coinShopDataVOs=new List<CoinShopDataVO>();
    private void Awake()
    {
      //  Debug.Log("INIT :" + API.IsInitialized());
        if (!API.IsInitialized())
        {
            if (initializationInProgress == false)
            {

                initializationInProgress = true;
                //Initialize IAP
                API.Initialize(InitializeResult);
            }
        }
    }

    public override void Start()
	{
        //if ((bool)ImageBackBlocking)
        //{
        //	ImageBackBlocking.gameObject.SetActive(value: true);
        //	ImageBackBlocking.color = new Color(0f, 0f, 0f, 0f);
        //	//ImageBackBlocking.DOFade(0.5f, 0.5f);
        //}
        base.Start();

        loopScroll_Ctrl.scrollEnterEvent = (realIndex, rowIndex, columnIndex, target) => {
            CoinShopDataVO coinShopData = coinShopDataVOs[realIndex];
            GameObject goods_Case = target.gameObject;// Instantiate(goods_Prefab, goods_Prefab.transform.parent);

            goods_Case.gameObject.SetActive(true);
            Sprite frame_Sprite = sprite_Group["Frame_Default"];
            Sprite banner_Sprite = null;
            string banner_Text = "";
            Image banner_Image = goods_Case.transform.GetChild<Image>("Banner_Image");
            if (realIndex == 1)
            {
                frame_Sprite = sprite_Group["Frame_Blue"];
                banner_Sprite = sprite_Group["Banner_Blue"];
                banner_Text = "Most\r\nPopular";
                banner_Image.gameObject.SetActive(true);
            }
            else if (realIndex >= coinShopDataVOs.Count - 1)
            {
                frame_Sprite = sprite_Group["Frame_Pink"];
                banner_Sprite = sprite_Group["Banner_Pink"];
                banner_Text = "Best\r\nValue";
                banner_Image.gameObject.SetActive(true);
            }
            else
            {
                banner_Image.gameObject.SetActive(false);
            }
            goods_Case.GetComponent<Image>().sprite = frame_Sprite;
            banner_Image.sprite = banner_Sprite;
            goods_Case.transform.GetChild<Text>("Banner_Text").text = banner_Text;
            goods_Case.transform.GetChild<Button>("Buy_Button").onClick.AddListener(() => {
                uipopupTreasureShopApi.MakeBuyProductThrillGame(coinShopData.rechargeAmount);
            });
            goods_Case.transform.GetChild<Text>("Buy_Button/Text").text = "$" + coinShopData.rechargeAmount.ToString();
            goods_Case.transform.GetChild<Text>("Price_Text").text = coinShopData.rechargeAmount.ToString() + "+" + coinShopData.awardAmount.ToString();
            Button button = target.transform.GetChild<Button>("Buy_Button");
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => {
                uipopupTreasureShopApi.MakeBuyProductThrillGame(coinShopDataVOs[realIndex].rechargeAmount);
            });
        };
        loopScroll_Ctrl.Init(0,0);
        goods_Prefab.gameObject.SetActive(false);
        UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(getCoinShopDataUrl, null, (resultData) =>
        {
            ReturnData<PageResultPacketSendRespVO<CoinShopDataVO>> result = JsonConvert.DeserializeObject<ReturnData<PageResultPacketSendRespVO<CoinShopDataVO>>>(resultData);
            coinShopDataVOs.Clear();
            coinShopDataVOs.AddRange(result.data.list);
            loopScroll_Ctrl.Refresh(coinShopDataVOs.Count);
        }, () =>
        {
            coinShopDataVOs.Clear();
            loopScroll_Ctrl.Refresh(coinShopDataVOs.Count);
            PopupManager.Instance.Close();
        });
    }

    private void InitializeResult(IAPOperationStatus status, string message, List<StoreProduct> shopProducts)
	{
       // Debug.Log("Chay vao day");
        initializationInProgress = false;
        consumableProducts = new List<DM_StoreProducts>();
        if (status == IAPOperationStatus.Success)
        {
            //IAP was successfully initialized
            //loop through all products and check which one are bought to update our variables
            for (int i = 0; i < shopProducts.Count; i++)
            {
                if (shopProducts[i].productName == "Coin1")
                {
                    //if a product is active, means that user had already bought that product so enable access
                    if (shopProducts[i].active)
                    {
                        //unlockLevel1 = true;
                    }
                }
                if (shopProducts[i].productName == "Coin2")
                {
                    if (shopProducts[i].active)
                    {
                        //unlockLevel2 = true;
                    }
                }
                if (shopProducts[i].productName == "Coin3")
                {
                    //if a subscription is active means that the subscription is still valid so enable access
                    if (shopProducts[i].active)
                    {
                        //subscription = true;
                    }
                }
                if (shopProducts[i].productName == "Coin4")
                {
                    //if a subscription is active means that the subscription is still valid so enable access
                    if (shopProducts[i].active)
                    {
                        //subscription = true;
                    }
                }
                if (shopProducts[i].productName == "Coin5")
                {
                    //if a subscription is active means that the subscription is still valid so enable access
                    if (shopProducts[i].active)
                    {
                        //subscription = true;
                    }
                }

                //construct a different list of each category of products, for an easy display purpose, not required
                switch (shopProducts[i].productType)
                {
                    case ProductType.Consumable:
                        consumableProducts.Add(new DM_StoreProducts(API.ConvertNameToShopProduct(shopProducts[i].productName), shopProducts[i].active));
                        break;

                }
            }

                
            }

       
    }
	

    public void MakeBuyProduct(int indexProduct)
    {

        switch(indexProduct)
        {
            case 1:
                {
                    API.BuyProduct(ShopProductNames.Coin1, ProductBought);
                    break;
                }
            case 2:
                {
                    API.BuyProduct(ShopProductNames.Coin2, ProductBought);
                    break;
                }
            case 3:
                {
                    API.BuyProduct(ShopProductNames.Coin3, ProductBought);
                    break;
                }
            case 4:
                {
                    API.BuyProduct(ShopProductNames.Coin4, ProductBought);
                    break;
                }
            case 5:
                {
                    API.BuyProduct(ShopProductNames.Coin5, ProductBought);
                    break;
                }
            case 6:
                {
                    API.BuyProduct(ShopProductNames.Removeads, ProductBought);
                    break;
                }

        }
    
    }

    

    /// <summary>
    /// automatically called after one product is bought
    /// </summary>
    /// <param name="status">The purchase status: Success/Failed</param>
    /// <param name="message">Error message if status is failed</param>
    /// <param name="product">the product that was bought, use the values from shop product to update your game data</param>
    private void ProductBought(IAPOperationStatus status, string message, StoreProduct product)
    {
        purchaseInProgress = false;
        if (status == IAPOperationStatus.Success)
        {
            if (Gley.EasyIAP.Internal.IAPManager.Instance.debug)
            {
                Debug.Log("Buy product completed: " + product.localizedTitle + " receive value: " + product.value);
                //GleyEasyIAP.ScreenWriter.Write("Buy product completed: " + product.localizedTitle + " receive value: " + product.value);
            }

            //each consumable gives coins in this example
            if (product.productType == ProductType.Consumable)
            {
                
                //coins += product.value;
                base.OnEventClose();
                MonoSingleton<PlayerDataManager>.Instance.IncreaseCoin(product.value);
                SoundSFX.Play(SFXIndex.DailyBonusGet);
                MonoSingleton<UIOverlayEffectManager>.Instance.ShowEffectRibbonFireworks();

                Toast.Show("Successful purchase " + product.value + " coins!", 3f, ToastColor.Green);

            }

            else if(product.productType == ProductType.NonConsumable)
            {
                Gley.MobileAds.APIMobileAds.RemoveAds(true);
                Toast.Show("Successful remove ads !", 3f, ToastColor.Green);
            }
        }
        else
        {
            //en error occurred in the buy process, log the message for more details
            if (Gley.EasyIAP.Internal.IAPManager.Instance.debug)
            {
                Debug.Log("Buy product failed: " + message);
                //GleyEasyIAP.ScreenWriter.Write("Buy product failed: " + message);
            }
        }
    }



    public void OnClickWatchAds()
    {
        if (Gley.MobileAds.APIMobileAds.IsRewardedVideoAvailable())
        {
			Gley.MobileAds.APIMobileAds.ShowRewardedVideo(CompleteMethod);
		}
		
	}


	private void CompleteMethod(bool completed)
	{
		  
		if (completed == true)
		{
			//Debug.Log("Chay vao say");
			//MonoSingleton<PlayerDataManager>.Instance.IncreaseCoin(10);
			base.OnEventClose();
            PopupRewardCoins popupRewardCoins = MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupRewardCoins, enableBackCloseButton: true) as PopupRewardCoins;
        }
		else
		{
			Debug.Log("No Reward");
		}
	}


    public override void OnEventClose()
    {
        base.OnEventClose();
        UIManager.holdOnUpdateCoin = false;
        MonoSingleton<UIManager>.Instance.SetCoinCurrencyMenuLayer(isPopupOverLayer: false);
        if (MonoSingleton<PopupManager>.Instance.CurrentPopupType == PopupType.PopupInGameItemStore || (MonoSingleton<SceneControlManager>.Instance.CurrentSceneType == SceneType.Game && (MonoSingleton<PopupManager>.Instance.CurrentPopupType == PopupType.PopupGameOver || MonoSingleton<PopupManager>.Instance.CurrentPopupType == PopupType.PopupGameOverCollectInfo)))
        {
            MonoSingleton<UIManager>.Instance.SetCoinCurrencyMenuLayer(isPopupOverLayer: true);
        }
        else if (MonoSingleton<SceneControlManager>.Instance.CurrentSceneType == SceneType.Game)
        {
            MonoSingleton<UIManager>.Instance.HideCoinCurrentMenuLayer();
        }
        //if (purchaseReachedStep != AppEventManager.PurchaseReachedStep.Purchased)
        //{
        //    SendAppEventPurchaseFunnel(MonoSingleton<PlayerDataManager>.Instance.Coin, purchaseReachedStep);
        //}
        PeriodEventData.CheckAndOpenPopup();
    }

    private void OnDestroy()
	{
	}

    public class CoinShopDataVO
    {
        public int id;
        public float rechargeAmount;//³äÖµ½ð¶î
        public float awardAmount;//½±Àø½ð¶î
        public string createTime;
    }
}
