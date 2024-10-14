using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UserSaveReqVO
{
    public long id { get; set; }
    public string username { get; set; }
    public string nickname { get; set; }
    public string remark { get; set; }
    public long deptId { get; set; }
    public List<long> postIds { get; set; }
    public string email { get; set; }
    public string mobile { get; set; }
    public int sex { get; set; }
    public string avatar { get; set; }
    public string password { get; set; }
}