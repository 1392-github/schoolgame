using System.Text;
using UnityEngine;
using UnityEngine.Networking;
public static class Util
{
    static UTF8Encoding _encoding;
    public static UTF8Encoding encoding
    {
        get
        {
            if (_encoding == null)
            {
                _encoding = new UTF8Encoding();
            }
            return _encoding;
        }
    }
    public static T SendJSON<T>(string url, object obj)
    {
        UnityWebRequest request = new UnityWebRequest("https://1392year.pythonanywhere.com/sch/" + url, "POST");
        request.uploadHandler = new UploadHandlerRaw(encoding.GetBytes(JsonUtility.ToJson(obj)));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SendWebRequest();
        while (!request.isDone);
        T result = JsonUtility.FromJson<T>(request.downloadHandler.text);
        request.Dispose();
        return result;
    }
    public static T SendJSON2<T>(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get("https://1392year.pythonanywhere.com/sch/" + url);
        //UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:3000/sch/" + url);
        request.SendWebRequest();
        while (!request.isDone) ;
        if (request.responseCode == 200)
        {
            T result = JsonUtility.FromJson<T>(request.downloadHandler.text);
            request.Dispose();
            return result;
        }
        else
        {
            request.Dispose();
            return default;
        }
    }
}
