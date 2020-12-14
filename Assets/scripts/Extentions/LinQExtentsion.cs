using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LinQExtentsion
{
    public static void MoveFirstToEndOfList<T>(this List<T> list)
    {
        T item = list[0];
        list.RemoveAt(0);
        list.Add(item);
    }

    public static List<T> FindClosestElements<T>(this List<T> list, int id)
    {
        List<T> elements = new List<T>();
        if (list.ElementAt(id-1) != null)
            elements.Add(list.ElementAt(id-1));
        if (list.ElementAt(id+1) != null)
            elements.Add(list.ElementAt(id+1));
        return elements;
    }
}
