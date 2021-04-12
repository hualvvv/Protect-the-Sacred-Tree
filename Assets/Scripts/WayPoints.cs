using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    //全てのWayPointsのtransformを取得する.
    public static Transform[] points;

    private void Awake()
    {
        points = new Transform[transform.childCount];
        for(int i = 0; i< points.Length; i++)
        {
            points[i] = transform.GetChild(i);
        }
    }
}
