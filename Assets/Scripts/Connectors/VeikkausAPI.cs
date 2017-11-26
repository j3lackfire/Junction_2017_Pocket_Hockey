using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class VeikkausAPI {
    public static readonly string headerKey = "X-ESA-API-Key";
    public static readonly string headerKeyValue = "ROBOT";

    public static readonly string baseURL = "https://www.veikkaus.fi/api/v1/sports/3/?";

    public static void SetWebRequestHeader(this UnityWebRequest _request)
    {
        _request.SetRequestHeader(headerKey, headerKeyValue);
    }

    public static string GetBaseURL()
    {
        return baseURL;
    }

    public static string AddLanguageEnding(this string _baseURL)
    {
        return _baseURL + "lang=fi";
    }
}
