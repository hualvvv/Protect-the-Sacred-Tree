using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetMoney : MonoBehaviour
{
    //敵が死亡する時、相応の金を獲得する。
    private void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
        transform.Translate(0, 1*Time.deltaTime, 0);
    }
}
