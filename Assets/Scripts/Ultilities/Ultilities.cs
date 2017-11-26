﻿using System;
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
}
