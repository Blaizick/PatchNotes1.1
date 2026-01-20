using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public static class Extensions
{
    public static List<T> Shuffle<T>(this List<T> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var j = UnityEngine.Random.Range(0, i);
            (list[i], list[j]) = (list[j], list[i]);
        }
        return list;
    }
}

public static class Utils
{
public static UnityEngine.Color FromHexadecimal(string hex)
{
    if (string.IsNullOrEmpty(hex))
        throw new System.ArgumentException("Hex string is null or empty");

    if (hex.StartsWith("#"))
        hex = hex.Substring(1);

    if (hex.Length != 6 && hex.Length != 8)
        throw new System.ArgumentException("Hex string must be 6 or 8 characters long");

    byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
    byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
    byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
    byte a = 255;

    if (hex.Length == 8)
        a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

    return new UnityEngine.Color32(r, g, b, a);
}
    public static bool MaskContainsLayer(LayerMask layerMask, int layer)
    {
        return (layerMask.value & (1 << layer)) > 0;       
    }
}