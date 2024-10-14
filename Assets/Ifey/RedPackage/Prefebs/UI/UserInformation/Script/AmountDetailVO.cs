using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AmountDetailVO
{
    public long Id;
    public long MemberId;
    public int TradeType;
    public double Amount;
    public string Mark;
    public string CreateTime;
}