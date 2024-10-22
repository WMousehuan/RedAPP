using Assets.Ifey.RedPackage.Prefebs.UI.Lobby.Channel.Cheasure.Scripts.Bean;
using Assets.Ifey.RedPackage.Prefebs.UI.Lobby.Channel.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PackageItem : MonoBehaviour
{
    public Sprite[] backgroundSprites; //add two pkg background img one for active 0 from unactive
    public Image backageImage;  //show item background
    
    public Text userName;
    public Text redAmount;
    public ImageNumberTools thunderNo;
    public Text compensateRatio;
    public GameObject hammerGameobject;
    public AvatarOfPlayer avatarOfPlayer;
    [HideInInspector]
    public string redStatus;  //redStatus 0 normal,1 over,2 timeout
    [HideInInspector]
    public PacketSendRespVO packetSendRespVO;

    public Text getIt_Text;
    public Button currentButton;
    public Text me_Text;

    private void Update()
    {
        if (getIt_Text && getIt_Text.gameObject.activeSelf)
        {
            getIt_Text.transform.localScale = Vector3.one + Vector3.one * Mathf.Sin(Time.time*6) * 0.1f;
        }
    }
    public void setBackground(int backageInt)
    {
        backageImage.sprite = backgroundSprites[backageInt];
    }

    public void setHammerVisi()
    {
        if (hammerGameobject == null)
        {
            return;
        }
        if (this.packetSendRespVO != null && this.packetSendRespVO.redStatus == 0)
        {
            hammerGameobject.SetActive(true);
        }
        else
        {
            hammerGameobject.SetActive(false);
        }
    }

    public void OnEventOpenPackageClick()
    {
        if (packetSendRespVO.isGrabed || packetSendRespVO.redStatus != 0)
        {
            OpenPackageDetailResultClick();
        }
        else
        {
            Popup popup = MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupTreasureOpen);
            PackageItem pkgItemPackageItem = popup.GetComponent<PackageItem>();
            pkgItemPackageItem.SetPacketSendRespVOInfo(this.packetSendRespVO);
        }

    }
    public void OpenPackageDetailResultClick()
    {
        Popup popup = MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupTreasureResultDetailOpen);
        UIPopupTreasureDetailResultMain uIPopupTreasureDetailResultMain = popup.GetComponent<UIPopupTreasureDetailResultMain>();
        uIPopupTreasureDetailResultMain.myPackageItem = this;
        uIPopupTreasureDetailResultMain.setPacketInfor(this.packetSendRespVO);
    }
    public void SetPacketSendRespVOInfo()
    {
        SetPacketSendRespVOInfo(packetSendRespVO);
    }
    public void SetPacketSendRespVOInfo(PacketSendRespVO packetSendRespVO)
    {
        if (packetSendRespVO == null)
        {
            this.me_Text?.gameObject?.SetActive(false);
            this.packetSendRespVO = null;
            this.userName.text = "Loading..";
            this.redAmount.text = "Loading..";
            this.compensateRatio.text = "Loading..";
            if (currentButton != null)
            {
                currentButton.interactable = false;
            }
            if (getIt_Text != null)
            {
                getIt_Text.gameObject.SetActive(false);
            }
            avatarOfPlayer.SetDefaultAvatar();
            backageImage.color = new Color(0.5f, 0.5f, 0.5f, 1);
            setBackground(1);
            this.transform.GetChild("Fx_Star").gameObject?.SetActive(false);
            //setBackground(0);
            return;
        }
        if (currentButton != null)
        {
            currentButton.interactable = true;
        }
        this.me_Text?.gameObject?.SetActive(packetSendRespVO.memberId==UserManager.Instance.appMemberUserInfoRespVO.id);
        if (!string.IsNullOrEmpty(packetSendRespVO.Avatar))
        {
            avatarOfPlayer.StartToGetUrlImage(packetSendRespVO.Avatar);
        }
        this.packetSendRespVO = packetSendRespVO;
        string nickNameToShow = string.IsNullOrEmpty(packetSendRespVO.nickName) ? "noname" : packetSendRespVO.nickName;
        //Debug.Log("usernameToShow" + usernameToShow);
        this.userName.text = nickNameToShow;
        this.redAmount.text = packetSendRespVO.redAmount.ToString();
        this.thunderNo.setNumber(packetSendRespVO.thunderNo);
        this.compensateRatio.text = packetSendRespVO.compensateRatio.ToString() + "X";
        redStatus = packetSendRespVO.redStatus.ToString();

        bool canGrab = packetSendRespVO.redStatus == 0 && !packetSendRespVO.isGrabed;
        if (getIt_Text != null)
        {
            getIt_Text.gameObject.SetActive(canGrab);
        }
        if (canGrab)
        {
            this.transform.GetChild("Fx_Star").gameObject?.SetActive(true);
            backageImage.color = Color.white;
            setBackground(0);
        }
        else
        {
            this.transform.GetChild("Fx_Star").gameObject?.SetActive(false);
            backageImage.color = new Color(0.5f, 0.5f, 0.5f, 1);
            setBackground(1);
        }
        
       
        setHammerVisi();
    }

}
