using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombNumberCheckBox : MonoBehaviour
{
    public Text numberText;
    public Text compentstateRatioText;
    public Toggle numberToggle;

    List<BombNumberCheckBox> playersCheckBoxList; //all checkBoxList
    public long id { get; set; }
    public int redPacketNum { get; set; }
    public double compensateRatio { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        //numberText = GetComponentInChildren<Text>();
        //numberToggle = GetComponentInChildren<Toggle>();
        numberToggle.onValueChanged.AddListener(OnNumberToggleValueChanged);
    }

    // Update is called once per frame
    public void setPlayersCheckBoxList(List<BombNumberCheckBox> playersCheckBoxList)
    {
        this.playersCheckBoxList = playersCheckBoxList;
    }
    public void setNumberText(string num)
    {
        numberText.text = num;
    }
    public void setCompensateRatioText(string compensateRatioText)
    {
        compentstateRatioText.text = compensateRatioText;
    }
    //checkBoxValueChange
    void OnNumberToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            if (playersCheckBoxList != null)
            {
                foreach (var item in playersCheckBoxList)
                {
                    if (item.id != id) {
                        item.numberToggle.isOn = false;
                    }
                }
            }
            // Toggle ��ѡ��ʱ���߼�
            Debug.Log("Toggle is turned on");
        }
        else
        {
            // Toggle ��ȡ��ѡ��ʱ���߼�
            Debug.Log("Toggle is turned off");
        }
    }

    public void SetToggleOn()
    {
        numberToggle.isOn = true; // ���� Toggle Ϊѡ��״̬
    }

    public void SetToggleOff()
    {
        numberToggle.isOn = false; // ���� Toggle Ϊδѡ��״̬
    }
    //public void initNumberText()
    //{
    //    numberText.text = redPacketNum.ToString();
    //}
    public bool getToggleIsOn()
    {
        return numberToggle.isOn;
    }
}
