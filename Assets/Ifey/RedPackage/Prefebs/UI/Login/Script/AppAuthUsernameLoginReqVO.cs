using System.Collections;
using UnityEngine;

namespace Assets.Ifey.RedPackage.Prefebs.UI.Login.Script
{
    public class AppAuthUsernameLoginReqVO
    {

        public string username { get; set; }
        public string password { get; set; }
    }
    public class AppAuthPhoneNumberReqVO
    {

        public string mobile { get; set; }
        public string code { get; set; }
    }
}