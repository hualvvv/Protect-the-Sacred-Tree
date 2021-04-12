using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyWave
{
    //各波の敵の異なるパラメータを保存する。
    public GameObject enemyPrefab;
    public int count;
    public float rate;
}
