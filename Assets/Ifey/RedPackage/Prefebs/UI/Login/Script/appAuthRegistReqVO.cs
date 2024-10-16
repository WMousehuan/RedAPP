using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AppAuthRegistReqVO
{
    public long id { get; set; }
    public string username { get; set; }
    public string nickname { get; set; }
    public string password { get; set; }
    public string payPassword { get; set; }

    public string encryptSuperiorId { get; set; }
}