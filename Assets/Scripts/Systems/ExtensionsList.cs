using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionsList
{
    //Take out a random card of the list and return it, the card will then be removed from the list
    public static T Draw<T>(this List<T> list)
    {
        if (list.Count == 0) return default;

        int randomCard = Random.Range(0, list.Count);

        T card = list[randomCard];

        list.Remove(card);

        return card;
    }

    public static T Shuffle<T>(this List<T> list)
    {
        if (list.Count == 0) return default;

        for (int i = 0;i < list.Count - 1; i++)
        {
            T temp = list[i];
            int random = Random.Range(i, list.Count);
            list[i] = list[random];
            list[random] = temp;
        }
        return list[list.Count - 1];
    }
}
