using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    //弾のパラメータを設定する。
    //弾のダメージ。
    private float damage = 50;
    //爆裂のエフェクト。
    public GameObject explosionEffect;
    //弾の種類を設定する。
    public enum E_DamageType {
        Fire,
        ICE
    };
    public E_DamageType damageType;
    //冷凍エフェクトを取得
    public GameObject iceEffect;
    //Fire,Iceという2種類の攻撃手段の砲弾のビジュアルを取得する。
    public GameObject fireShoot;
    public GameObject iceShoot;

    //敵とぶつかる時、弾の種類によって、異なる効果を表す。
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            switch (damageType)
            {
                case E_DamageType.Fire:
                    other.GetComponent<EnemyController>().Attacked(damage);
                    GameObject explosionEff = Instantiate(explosionEffect, transform.position, Quaternion.identity);
                    Destroy(explosionEff, 1f);
                    Destroy(gameObject);
                    break;
                case E_DamageType.ICE:
                    other.GetComponent<EnemyController>().Freeze(0.8f);
                    GameObject iceEff = Instantiate(iceEffect, other.transform.position, Quaternion.identity);
                    Destroy(iceEff, 0.8f);
                    Destroy(gameObject);
                    break;
                default: break;
            }
        }
    }
}
