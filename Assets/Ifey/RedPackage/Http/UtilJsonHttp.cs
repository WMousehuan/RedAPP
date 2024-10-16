using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;
public class UtilJsonHttp : MonoSingleton<UtilJsonHttp>
{
    //测试
    string mainDomain = "http://43.198.88.230:48090";

    //本地
    //string mainDomain = "http://192.168.1.31:48090"; 

    //线上
    //string mainDomain = "http://192.168.1.31:48090"; 

    /// <summary>
    /// GetRequset
    /// </summary>
    /// <param name="apiUrl"></param>
    /// <param name="httpInterface"></param>
    public void GetRequestWithAuthorizationToken(string apiUrl, HttpInterface httpInterface,System.Action<string> successAction =null, System.Action failAction = null)
    {
        StartCoroutine(IEGetRequestWithAuthorizationToken(apiUrl, httpInterface, successAction, failAction));
    }
    IEnumerator IEGetRequestWithAuthorizationToken(string apiUrl, HttpInterface httpInterface, System.Action<string> finishAction = null, System.Action failAction = null)
    {
        string url = mainDomain+ apiUrl; 
        // Create UnityWebRequest Object
        using (UnityWebRequest www = new UnityWebRequest(url, "GET"))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", RedPackageAuthor.Instance.authorizationValue); //token
            Debug.Log("IESendRequestWithAuthorizationToken Authorization="+ RedPackageAuthor.Instance.authorizationValue);
            // 发送请求
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                httpInterface?.UnknowError(www.error);
                failAction?.Invoke();
            }
            else
            {
                string result = www.downloadHandler.text;
                Debug.Log(result+"\r\n"+ httpInterface?.GetType().ToString());
                // Use Json.NET to JSON the result to JObject type
                JObject json = JObject.Parse(result);
                int code = json["code"].Value<int>();
                if (code == 0)
                {
                    httpInterface?.Success(result);
                    finishAction?.Invoke(result);
                    //responseData = JsonConvert.DeserializeObject<ReturnData<Object>>(result);
                }
                else
                {
                    httpInterface?.Fail(json);
                    failAction?.Invoke();
                }
            }
        }
    }

    public async void GetRequestWithAuthorizationTokenSync(string apiUrl, HttpInterface httpInterface)
    {
        string url = mainDomain + apiUrl;

        // Create UnityWebRequest Object
        UnityWebRequest www = new UnityWebRequest(url, "GET");
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Authorization", RedPackageAuthor.Instance.authorizationValue); //token
        Debug.Log("SendRequestWithAuthorizationToken Authorization=" + RedPackageAuthor.Instance.authorizationValue);
        // Send request synchronously
        await www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            httpInterface?.UnknowError(www.error);
        }
        else
        {
            string result = www.downloadHandler.text;
            Debug.Log(result);
            // Use Json.NET to JSON the result to JObject type
            JObject json = JObject.Parse(result);
            int code = json["code"].Value<int>();
            if (code == 0)
            {
                httpInterface?.Success(result);
                //responseData = JsonConvert.DeserializeObject<ReturnData<Object>>(result);
            }
            else
            {
                httpInterface?.Fail(json);
            }
        }
    }

    public void PostRequestWithParamAuthorizationToken(string apiUrl, object paramers, HttpInterface httpInterface, System.Action<string> successAction = null, System.Action failAction = null)
    {
        StartCoroutine(IEPostRequestWithParamAndToken(apiUrl, paramers, httpInterface, successAction, failAction));
    }
    IEnumerator IEPostRequestWithParamAndToken(string apiUrl, object paramers, HttpInterface httpInterface, System.Action<string> successAction = null, System.Action failAction = null)
    {
        string url = mainDomain + apiUrl;

        // Serialize paramers object to JSON
        string jsonParam = JsonConvert.SerializeObject(paramers);
        print(jsonParam);
        // Create UnityWebRequest Object
        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonParam);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", RedPackageAuthor.Instance.authorizationValue); //token

            // Send request
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                failAction?.Invoke();
            }
            else
            {
                string result = www.downloadHandler.text;
                Debug.Log(result);
                // Use Json.NET to JSON the result to JObject type
                JObject json = JObject.Parse(result);
                int code = json["code"].Value<int>();
                if (code == 0)
                {
                    httpInterface?.Success(result);
                    successAction?.Invoke(result);
                    //responseData = JsonConvert.DeserializeObject<ReturnData<Object>>(result);
                }
                else
                {
                    httpInterface?.Fail(json);
                    failAction?.Invoke();
                }
            }
        }
    }
    public void PostFileWithParamAuthorizationToken(string apiUrl, string fileName, byte[] fileData, HttpInterface httpInterface, System.Action<string> successAction = null, System.Action failAction = null)
    {
        StartCoroutine(IEPostFileWithParamAndToken(apiUrl,  fileData, httpInterface, successAction, failAction));
    }
    IEnumerator IEPostFileWithParamAndToken(string apiUrl, byte[] fileData, HttpInterface httpInterface, System.Action<string> successAction = null, System.Action failAction = null)
    {
        string url = mainDomain + apiUrl;

        WWWForm form = new WWWForm();//使用WWWForm将文件作为一个表单上传服务器
                                     //键值对
        form.AddBinaryData("file", fileData);//添加二进制字节流到表单
        // Create UnityWebRequest Object
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Connection", "keep-alive");
            www.SetRequestHeader("Authorization", RedPackageAuthor.Instance.authorizationValue); //token
            // Send request
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                failAction?.Invoke();
            }
            else
            {
                string result = www.downloadHandler.text;
                Debug.Log(result);
                // Use Json.NET to JSON the result to JObject type
                JObject json = JObject.Parse(result);
                int code = json["code"].Value<int>();
                if (code == 0)
                {
                    httpInterface?.Success(result);
                    successAction?.Invoke(result);
                    //responseData = JsonConvert.DeserializeObject<ReturnData<Object>>(result);
                }
                else
                {
                    httpInterface?.Fail(json);
                    failAction?.Invoke();
                }
            }
        }
    }
    public void PutObjectWithParamAuthorizationToken(string apiUrl, object paramers, HttpInterface httpInterface, System.Action<string> successAction = null, System.Action failAction = null)
    {
        string jsonParam = JsonConvert.SerializeObject(paramers);
        StartCoroutine(IEPutContentWithParamAndToken(apiUrl, jsonParam, httpInterface, successAction, failAction));
    }
    public void PutContentWithParamAuthorizationToken(string apiUrl, string jsonParam, HttpInterface httpInterface, System.Action<string> successAction = null, System.Action failAction = null)
    {
        StartCoroutine(IEPutContentWithParamAndToken(apiUrl, jsonParam, httpInterface, successAction, failAction));
    }
    IEnumerator IEPutContentWithParamAndToken(string apiUrl, string jsonParam, HttpInterface httpInterface, System.Action<string> successAction = null, System.Action failAction = null)
    {
        string url = mainDomain + apiUrl;

        print(jsonParam);
        // Create UnityWebRequest Object
        using (UnityWebRequest www = new UnityWebRequest(url, "PUT"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonParam);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();

            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", RedPackageAuthor.Instance.authorizationValue); //token

            // Send request
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                failAction?.Invoke();
            }
            else
            {
                string result = www.downloadHandler.text;
                Debug.Log(result);
                // Use Json.NET to JSON the result to JObject type
                JObject json = JObject.Parse(result);
                int code = json["code"].Value<int>();
                if (code == 0)
                {
                    httpInterface?.Success(result);
                    successAction?.Invoke(result);
                    //responseData = JsonConvert.DeserializeObject<ReturnData<Object>>(result);
                }
                else
                {
                    httpInterface?.Fail(json);
                    failAction?.Invoke();
                }
            }
        }
    }

    //public void PutFromWithParamAuthorizationToken(string apiUrl, (string, string) keyAndValue, HttpInterface httpInterface, System.Action<string> successAction = null, System.Action failAction = null)
    //{
    //    StartCoroutine(IEPutFormWithParamAndToken(apiUrl, keyAndValue, httpInterface, successAction, failAction));
    //}
    //IEnumerator IEPutFormWithParamAndToken(string apiUrl, (string,string) keyAndValue, HttpInterface httpInterface, System.Action<string> successAction = null, System.Action failAction = null)
    //{
    //    string url = mainDomain + apiUrl;

    //    WWWForm wWWForm = new WWWForm();
    //    //wWWForm.AddField
    //    wWWForm.AddBinaryData(keyAndValue.Item1, Encoding.UTF8.GetBytes(keyAndValue.Item2));
    //    // Create UnityWebRequest Object
    //    using (UnityWebRequest www = new UnityWebRequest(url, "PUT"))
    //    {
    //        //byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonParam);
    //        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
    //        www.downloadHandler = new DownloadHandlerBuffer();

    //        www.SetRequestHeader("Content-Type", "application/json");
    //        www.SetRequestHeader("Authorization", RedPackageAuthor.Instance.authorizationValue); //token

    //        // Send request
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log(www.error);
    //            failAction?.Invoke();
    //        }
    //        else
    //        {
    //            string result = www.downloadHandler.text;
    //            Debug.Log(result);
    //            // Use Json.NET to JSON the result to JObject type
    //            JObject json = JObject.Parse(result);
    //            int code = json["code"].Value<int>();
    //            if (code == 0)
    //            {
    //                httpInterface?.Success(result);
    //                successAction?.Invoke(result);
    //                //responseData = JsonConvert.DeserializeObject<ReturnData<Object>>(result);
    //            }
    //            else
    //            {
    //                httpInterface?.Fail(json);
    //                failAction?.Invoke();
    //            }
    //        }
    //    }
    //}
}
public static class MethodExpand
{
    public static TaskAwaiter GetAwaiter(this UnityWebRequestAsyncOperation result)
    {
        return new TaskAwaiter();
    }
}