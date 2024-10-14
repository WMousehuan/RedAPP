using System;

[Serializable]
public class AuthLoginReqVO
{
    public string username;
    public string password;
    public string captchaVerification;
    public int socialType;
    public string socialCode;
    public string socialState;
    public bool socialCodeValid;

    public AuthLoginReqVO(string username, string password, string captchaVerification, int socialType, string socialCode, string socialState, bool socialCodeValid)
    {
        this.username = username;
        this.password = password;
        this.captchaVerification = captchaVerification;
        this.socialType = socialType;
        this.socialCode = socialCode;
        this.socialState = socialState;
        this.socialCodeValid = socialCodeValid;
    }
}
