using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMark : MonoBehaviour
{
    //自身のビジュアルを取得する.
    public GameObject buildMarkPrefab;
    //colliderの半径とプレイヤーとの距離を取得します。プレイヤーを取得します。
    private float radius;
    private float distance;
    private GameObject player;
    //プレイヤーとの距離によってサイズが変わる効果を作りたい。
    //当時Markの辺の長さ。最大の長さ。
    private float size;
    private float maxLength =3.8f;
    //Mark自身の回転速度.
    private float rotateSpeed=2.5f;
    

    

    private void Start()
    {
        //buildMarkPrefabのmeshRendererを無効化にする。
        buildMarkPrefab.GetComponent<MeshRenderer>().enabled = false;
        //collider（Tigger）の半径を取得する。
        radius = GetComponent<SphereCollider>().radius;

        player = GameObject.Find("Player");
    }

    void changeSize()
    {
        Vector3 _mark = buildMarkPrefab.transform.position;
        Vector3 _player = player.transform.position;
        //キャラクターとMarkの距離を取得する。
        distance = Vector3.Distance(_player, _mark);
        //求距离减去半径的绝对值，并以此为判断大小的系数.
        //距離から半径の絶対値を差し引いて、この数字は半径に占める割合は現在Markの辺の長さは最大の長さに占める割合。
        //引き続き辺の長さを乗算し、現在表示されている長さを得る。
        size = Mathf.Abs(distance - radius)/radius*maxLength;
        buildMarkPrefab.transform.localScale=new Vector3(size,size,size);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            buildMarkPrefab.GetComponent<MeshRenderer>().enabled = true;
        }  
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            changeSize();
            markMovement();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            buildMarkPrefab.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void markMovement()
    {
        buildMarkPrefab.transform.Rotate(0,rotateSpeed, 0);
    }
}
