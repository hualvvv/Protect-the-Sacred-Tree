using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    //砲塔の回転部分を取得する。
    public GameObject turretHead;
    //攻撃間隔。
    public float attackRateTime = 1;
    public float timer = 0;
    //攻撃手段を設置する。
    public enum E_DamageType { 
        BULLET,
        ICELAZER
    };
    public E_DamageType damageType;
    //砲弾。
    public GameObject bulletPrefab;
    //攻撃時に砲弾を発射する位置を定義する。
    public Transform firePosition;
    //IceLazerという攻撃方式のパラメーター。
    public float iceDamage = 60;
    public LineRenderer iceLaserRenderer;
    public GameObject iceLaserEffect;
    //攻撃範囲内の全てのenemyを取得。
    public List<GameObject> enemys = new List<GameObject>();

    //攻撃範囲に入る時にリストを入れて、攻撃範囲を離れる時にリストから削除する。
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemys.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemys.Remove(other.gameObject);
        }
    }

    private void Start()
    {
        timer = attackRateTime;
        //IceLaserという攻撃手段のエフェクトをデフォルトで閉じる。
        if (iceLaserEffect != null)
        {
            iceLaserEffect.SetActive(false);
        }
    }

    private void Update()
    {
        //攻撃手段を選択/判断する。
        switch (damageType)
        {
            case E_DamageType.BULLET:
                //砲弾攻撃の場合。
                //時間が基準に達したら攻撃する。
                timer += Time.deltaTime;
                if (enemys.Count > 0 && timer >= attackRateTime)
                {
                    timer = 0;
                    Attack();
                }break;
            case E_DamageType.ICELAZER:
                //icelaser攻撃の場合。
                if (enemys.Count > 0)
                {
                    iceLaserEffect.SetActive(true);
                    IceLaserAttack();
                }
                else if (enemys.Count == 0)
                {
                    iceLaserRenderer.enabled = false;
                    GetComponent<AudioSource>().Stop();
                    iceLaserEffect.SetActive(false);
                }break;
        }

        //敵があれば回転して敵をロックする。
        if (enemys.Count > 0&& enemys[0] != null)
        {
            ChangeTurretRotation();
        }
    }

    //砲弾攻撃。
    void Attack()
    {
        if (enemys[0] == null)
        {
            UpdateEnemy();
        }
        if (enemys.Count > 0)
        {
            //砲弾を生成し、最初保存されたenemiyの位置を砲弾の攻撃目標として伝える。
            GameObject bullet = Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
            bullet.GetComponent<Bullet>().SetTarget(enemys[0].transform);

            GetComponent<AudioSource>().Play();
        }
        if (enemys.Count == 0)
        {
            timer = attackRateTime;
        }
    }

    void UpdateEnemy()
    {
        for (int i= enemys.Count-1; i >= 0; i--){
            if (enemys[i] == null)
            {
                enemys.RemoveAt(i);
            }
        }
        return;
    }

    //icelaser攻撃。
    void IceLaserAttack()
    {
        if (iceLaserRenderer.enabled == false)
        {
            iceLaserRenderer.enabled = true;
            GetComponent<AudioSource>().Play();
        }
        if (enemys[0] == null)
        {
            UpdateEnemy();
        }
        if (enemys.Count > 0)
        {
            iceLaserRenderer.SetPositions(new Vector3[] { firePosition.position, enemys[0].transform.position });
            enemys[0].GetComponent<EnemyController>().Attacked(iceDamage*Time.deltaTime);
            enemys[0].GetComponent<EnemyController>().IceDamage(0.5f);

            iceLaserEffect.transform.position = enemys[0].transform.position;
            iceLaserEffect.transform.LookAt(transform.position);
        }
    }

    void ChangeTurretRotation()
    {
        //砲塔の回転。
        Vector3 targetPosition = enemys[0].transform.position;
        targetPosition.y = turretHead.transform.position.y;
        turretHead.transform.LookAt(targetPosition);
    }
}
