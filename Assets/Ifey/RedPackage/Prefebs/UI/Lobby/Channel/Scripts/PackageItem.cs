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

    public Button currentButton;
    // Start is called before the first frame update
    public void setBackground(int backageInt)
    {
        backageImage.sprite = backgroundSprites[backageInt];
    }

    public void setBackgroundWithPacketSendRespVO()
    {
        if (this.packetSendRespVO !=null && this.packetSendRespVO.redStatus==0)
        {
            setBackground(0);
        }
        else
        {
            setBackground(1);
        }
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

    public void openPackageClick()
    {
        Popup popup = MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupTreasureOpen);
        PackageItem pkgItemPackageItem = popup.GetComponent<PackageItem>();
        pkgItemPackageItem.setPacketSendRespVOInfo(this.packetSendRespVO) ;
    }
    public void openPackageDetailResultClick()
    {
        Popup popup = MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupTreasureResultDetailOpen);
        UIPopupTreasureDetailResultMain uIPopupTreasureDetailResultMain = popup.GetComponent<UIPopupTreasureDetailResultMain>();
        uIPopupTreasureDetailResultMain.myPackageItem = this;
        uIPopupTreasureDetailResultMain.setPacketInfor(this.packetSendRespVO);
    }
    public void setPacketSendRespVOInfo(PacketSendRespVO packetSendRespVO)
    {
        if (packetSendRespVO == null)
        {
            this.packetSendRespVO = null;
            this.userName.text = "Loading..";
            this.redAmount.text = "Loading..";
            this.compensateRatio.text = "Loading..";
            if (currentButton != null)
            {
                currentButton.interactable = false;
            }
            avatarOfPlayer.SetDefaultAvatar();
            backageImage.color = new Color(1, 1, 1, 1);
            this.transform.GetChild("Fx_Star").gameObject?.SetActive(false);
            setBackground(0);
            return;
        }
        if (currentButton != null)
        {
            currentButton.interactable = true;
        }

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
        this.compensateRatio.text = packetSendRespVO.compensateRatio.ToString()+"X";
        redStatus = packetSendRespVO.redStatus.ToString();
        //Set the package color
        if (packetSendRespVO.redStatus == 0)
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
