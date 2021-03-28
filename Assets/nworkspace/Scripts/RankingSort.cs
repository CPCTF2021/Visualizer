using System.Collections.Generic;
using UnityEngine;

public class RankingSort : MonoBehaviour
{
    public static int CompareScore(Transform obj1, Transform obj2)
    {
        if (obj1 == null)
        {
            if (obj2 == null)
            {
                return 0;
            }
            return 1;
        }
        else
        {
            if (obj2 == null)
            {
                return -1;
            }
            int obj1score = int.Parse(obj1.name);
            int obj2score = int.Parse(obj2.name);
            return obj2score.CompareTo(obj1score);
        }
    }
    public void SortRankingList()
    {
        List<Transform> objList = new List<Transform>();
        int playerNum = transform.childCount;
        for (int i = 0; i < playerNum; i++)
        {
            objList.Add(transform.GetChild(i));
        }

        objList.Sort(CompareScore);

        foreach (var obj in objList)
        {
            obj.SetSiblingIndex(playerNum - 1);
        }
    }
}
