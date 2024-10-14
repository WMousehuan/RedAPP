using UnityEditor;
using UnityEngine;
    public class UserLoginReturnData
    {
        public long userId { get; set; }
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public long expiresTime { get; set; }
        public long? openid {  get; set; }
    }