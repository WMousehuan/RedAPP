using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class VerifyCodeTextCountDown_Ctrl : MonoBehaviour
{
    public Button button;

    public GameObject defualt_Case;

    public GameObject countDown_Case;

    [SerializeField]
    private Text text;
    [SerializeField]
    private TMP_Text tmp_Text;
    public long targetTimeStamp;

    public UserManager.VerifySceneType verifySceneType;
    public string content
    {
        set
        {
            if (text != null)
            {
                text.text = value;
            }
            if (tmp_Text != null)
            {
                tmp_Text.text = value;
            }
        }
    }

    void Start()
    {

        EventManager.Instance.Regist(GameEventType.SetVerifyStampTime.ToString(), this.GetInstanceID(), (objects) => {
            UserManager.VerifySceneType verifySceneType = (UserManager.VerifySceneType)objects[1];
            if (verifySceneType == this.verifySceneType)
            {
                targetTimeStamp = (int)objects[0];
            }
        }); 
        int currnetTimeSteamp = Utils.ConvertToTimestamp(DateTime.Now);
        targetTimeStamp = PlayerPrefs.GetInt("VerifyStampTime"+ verifySceneType.ToString());
        if (targetTimeStamp > currnetTimeSteamp)
        {
            button.interactable = false;
            countDown_Case.gameObject.SetActive(true);
            defualt_Case.gameObject.SetActive(false);
            content = (targetTimeStamp - currnetTimeSteamp).ToString();
        }
        else
        {
            button.interactable = true;
            countDown_Case.gameObject.SetActive(false);
            defualt_Case.gameObject.SetActive(true);
        }
    }
    private void OnDestroy()
    {
        EventManager.Instance.UnRegist(GameEventType.SetVerifyStampTime.ToString(), this.GetInstanceID());
    }
    // Update is called once per frame
    void Update()
    {
        int currnetTimeSteamp = Utils.ConvertToTimestamp(DateTime.Now);

        if (targetTimeStamp > currnetTimeSteamp)
        {
            if (button.interactable)
            {
                button.interactable = false;
                countDown_Case.gameObject.SetActive(true);
                defualt_Case.gameObject.SetActive(false);
            }
            content = (targetTimeStamp - currnetTimeSteamp).ToString();
        }
        else
        {
            if (!button.interactable)
            {
                button.interactable = true;
                countDown_Case.gameObject.SetActive(false);
                defualt_Case.gameObject.SetActive(true);
            }
        }
    }
}
