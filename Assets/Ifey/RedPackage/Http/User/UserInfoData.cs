using System;

[Serializable]
public class UserInfoData
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Nickname { get; set; }
    public string Remark { get; set; }
    public int DeptId { get; set; }
    public string DeptName { get; set; }
    public int PostIds { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public int Sex { get; set; }
    public string Avatar { get; set; }
    public int Status { get; set; }
    public string LoginIp { get; set; }
    public string LoginDate { get; set; }
    public string CreateTime { get; set; }
}