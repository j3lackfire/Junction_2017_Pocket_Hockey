using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class VeikkausConnector : BaseManager {

    public bool isLoading;

    public override void Init()
    {
        base.Init();
        //isLoading = false;
        //SendRequest();
    }

    public void SendRequest()
    {
        StartCoroutine(SendSearchRequest());
    }

    public IEnumerator SendSearchRequest()
    {
        isLoading = true;
        string requestString = VeikkausAPI.GetBaseURL().AddLanguageEnding();

        UnityWebRequest www = UnityWebRequest.Get(requestString);
        VeikkausAPI.SetWebRequestHeader(www);
        Debug.Log(requestString);

        yield return www.SendWebRequest();
        isLoading = false;

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("<color=red>Network error !!!</color>");
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            //Debug.Log("<color=cyan>Server response:</color>");
            Debug.Log(www.downloadHandler.text);
        }
    }


}
