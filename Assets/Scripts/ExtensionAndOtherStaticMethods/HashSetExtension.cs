using System.Collections.Generic;

public static class HashSetExtension
{
    public static T RemoveRandomElement<T>(this HashSet<T> data)
    {
        int elementIndex = UnityEngine.Random.Range(0, data.Count);
        int i = 0;
        foreach (var element in data)
        {
            if (i == elementIndex)
            {
                data.Remove(element);
                return element;
            }
            i++;
        }
        return default;
    }
}
