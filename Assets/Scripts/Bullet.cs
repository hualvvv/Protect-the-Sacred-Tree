using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //弾のパラメータを設定する。
    public float damage = 50;
    public float speed = 50f;
    //爆発のエフェクト。
    public GameObject explosionEffectPrefab;
    public GameObject fireExplosionEffectPrefab;
    //攻撃目標。
    private Transform targetEnemy;
    //砲弾の種類を選択する。
    public enum E_BulletType {
        FIRE,
        DEFAULT
    };
    public E_BulletType bulletType;

    //燃焼範囲を取得する。砲弾の種類はFIREを選択する場合、燃焼ダメージに変更する。
    public GameObject AoeFire;


    //弾丸攻撃ターゲット関数を設定する。
    public void SetTarget(Transform target)
    {
        targetEnemy = target;
    }

    //砲弾は目標の方に向けて撃つ。
    private void Update()
    {
        if (targetEnemy != null)
        {
            transform.LookAt(targetEnemy.position);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else if (bulletType==E_BulletType.DEFAULT) Destroy();
        else FireBulletDestroy();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            switch (bulletType)
            {
                case E_BulletType.DEFAULT:
                    //弾が爆発してダメージを与える.
                    other.GetComponent<EnemyController>().Attacked(damage);
                    Destroy();
                    break;
                case E_BulletType.FIRE:
                    //火炎弾が爆発してaoeダメージを与える。直接攻撃はダメージを与えない。
                    FireBulletAoe();
                    break;
            }
        }
    }

    //砲弾の種類はDEFAULTの場合の廃棄。
    private void Destroy()
    {
        GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        Destroy(effect, 1f);
        Destroy(gameObject);
    }

    //砲弾の種類はFIREの場合の廃棄。
    void FireBulletDestroy()
    {
        GameObject effect = Instantiate(fireExplosionEffectPrefab, transform.position, Quaternion.identity);
        Destroy(effect, 1f);
        Destroy(gameObject);
    }

    //砲弾の種類はFIREの場合、範囲内で炎が発生する。
    void FireBulletAoe()
    {
        GameObject aoeFire = Instantiate(AoeFire, transform.position, Quaternion.identity);
        Destroy(aoeFire, 3f);
        Destroy(gameObject);
    }
}
