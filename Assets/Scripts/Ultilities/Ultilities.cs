using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ultilities{

    private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static long GetCurrentUnixTimestampMillis()
    {
        return (long)(DateTime.UtcNow - UnixEpoch).TotalMilliseconds;
    }

    public static DateTime DateTimeFromUnixTimestampMillis(long millis)
    {
        return UnixEpoch.AddMilliseconds(millis);
    }

    //min shooting power should be 2
    //max should be like like 10, or even 5
    public static float CalculateShootingPower(Vector3 v1, Vector3 v2)
    {
        float f = Vector3.Distance(v1, v2) / 12f;
        if (f <= 2f)
        {
            f = 2f;
        }
        if (f >= 5f)
        {
            f = 5f;
        }
        return f;
    }
}


public class ReadOnlyAttribute : PropertyAttribute
{

}
