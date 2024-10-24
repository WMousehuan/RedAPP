using System;
using System.Collections.Generic;
using System.Xml.Schema;

[Serializable]
public class ReturnData<T>
{
    public int code;
    public T data;
    public string msg;
}


/// <summary>
/// 获取分页的数据
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class PageResultPacketSendRespVO<T>
{
    public T[] list;
    public int total;
}

public class PostFileData
{
    public byte[] file = null;

    public PostFileData( byte[] file)
    {
        this.file = file;
    }
}