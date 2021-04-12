using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    //カメラ回転スビート。
    private float y=1;
    void Update()
    {
        transform.Rotate(0, y*Time.deltaTime, 0);
    }
}
