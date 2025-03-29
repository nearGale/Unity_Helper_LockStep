using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FixedPointNumber
{
    public static long GetFixedPointLong(float val)
    {
        long finalVal = (long)(val * 1000);
        return finalVal;
    }

    public static float GetFixedPointFloat(long val)
    {
        float finalVal = (float)val / 1000f;
        return finalVal;
    }
}
