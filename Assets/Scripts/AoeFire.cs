using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeFire : MonoBehaviour
{
    //攻撃のダメージと持続時間、エフェクト.
    public float damage = 50;
    public float duration = 3f;
    public GameObject aoeFireEffect;
    private void OnTriggerStay(Collider other)
    {
        //敵はスキル範囲内の時.
        if (other.tag == "Enemy")
        {
            //毎秒ダメージをあたえます。攻撃された敵に火のエフェクトがつく。
            other.GetComponent<EnemyController>().Attacked(damage*Time.deltaTime);
            GameObject aoeFireEff=Instantiate(aoeFireEffect,other.transform.position,Quaternion.identity);
            Destroy(aoeFireEff, 0.2f);
            Destroy(gameObject,duration);
        }
    }
}
