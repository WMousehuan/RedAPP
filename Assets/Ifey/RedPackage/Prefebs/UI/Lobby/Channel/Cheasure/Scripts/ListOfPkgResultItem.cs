using Assets.Ifey.RedPackage.Prefebs.UI.Lobby.Channel.Cheasure.Scripts.Bean;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListOfPkgResultItem : MonoBehaviour
{
    public Image backageImage;  //show item background
    public Sprite[] backgroundSprites; //add two pkg background img one for bomb and normal
    public AvatarOfPlayer avatarOfPlayer; //Avatar
    public Text userName;
    public Text createTime;
    public Text grabCoin;
    public GameObject bombCoinGameObject;
    public Text bombCoin;
    public Text me_Text;
    // Start is called before the first frame update
    public void SetResultItemDetailValue(AppPacketReceiveRespVO pkgDetailItem)
    {
        me_Text?.gameObject?.SetActive(pkgDetailItem.MemberId == UserManager.Instance.appMemberUserInfoRespVO.id);
        userName.text = pkgDetailItem.NickName;
        grabCoin.text = pkgDetailItem.GetAmount.ToString();
        createTime.text = pkgDetailItem.CreateTimeDateTime.ToString();
        //Debug.Log("(pkgDetailItem.CompensateAmount========"+ pkgDetailItem.CompensateAmount);
        //SHow bomb
        if (pkgDetailItem.CompensateAmount != null&& pkgDetailItem.CompensateAmount < 0)
        {
            setBackground(1);
            setBombObjectActivity(true);
            bombCoin.text = pkgDetailItem.CompensateAmount.ToString();
        }
        else
        {
            setBackground(0);
            setBombObjectActivity(false);
        }
        if (!string.IsNullOrEmpty(pkgDetailItem.Avatar))
        {
            avatarOfPlayer.StartToGetUrlImage(pkgDetailItem.Avatar);
        }

    }

    public void setBackground(int backageInt)
    {
        backageImage.sprite = backgroundSprites[backageInt];
    }

    public void setBombObjectActivity(bool value)
    {
        bombCoinGameObject.SetActive(value);
    }
}
