using System.Collections.Generic;

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