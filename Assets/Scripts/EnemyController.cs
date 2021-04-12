using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //パラメータを設定する。
    public float moveSpeed = 1.0f;
    private float originalMoveSpeed;
    public float hp = 200;
    public float totalHp;
    //前進の目標。
    private Transform[] points;
    //第何個の目標点。
    private int index = 0;
    //死亡時エフェクト。
    public GameObject explosionEffect;
    //凍りついていますか。
    private bool isFreezing = false;
    //HP。
    private Slider hpSlider;
    public GameObject canvas;
    //Navigation.
    private NavMeshAgent navMeshAgent;
    //運動アニメーション。
    private Animator animator;
    //敵が消滅した後に獲得する資金の数と獲得する時のヒント。
    public float getMoney=20;
    public GameObject getMoneyText;
    //各種音声。
    public AudioSource fireAudio;
    public AudioSource iceAudio;
    public AudioSource deathAudio;

    void Start()
    {
        points = WayPoints.points;
        originalMoveSpeed = moveSpeed;

        hpSlider = GetComponentInChildren<Slider>();
        totalHp = hp;

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
      
        getMoneyText.GetComponentInChildren<Text>().text="+" + getMoney;
    }

    void Update()
    {
        navMeshAgent.speed = moveSpeed;
        animator.SetFloat("Speed", moveSpeed);

        Move();

        //敵のHPがカメラに向く。
        if (hpSlider != null)
        {
            hpSlider.transform.LookAt(Camera.main.transform);
            hpSlider.transform.Rotate(0, 180, 0);
        }
    }

    void Move()
    {
        //到着したPointは最後の目的地でしたらゲーム失敗。
        //到着したPointは最後の目的地ではないなら、次のPointに進みます。
        if (index > points.Length-1) {
            Destination();
            return;   
        };
        navMeshAgent.SetDestination(points[index].position);

        if (Vector3.Distance(points[index].position, transform.position) < 2f)
        {
            index++;
        }
    }

    //目的地に着いたら、ゲームオーバー。自分を廃棄する。
    private void Destination()
    {
        Destroy(gameObject);
        GameManager.instance.Defeat();
    }

    //破壊されると、生き残った敵の数が1つ減ります。
    private void OnDestroy()
    {
        EnemySpawner.enemyAliveCount--;
    }

    //弾丸に当たったとき。
    public void Attacked(float damage)
    {
        if (hp <= 0) return;
        hp -= damage;
        hpSlider.value = hp / totalHp;

        if (damage > 30)
        {
            fireAudio.Play();
        }

        if (hp <= 0)
        {
            Die();
        }
    }

    //IceLaserに当たって減速する。
    public void IceDamage(float DecelerationRatio)
    {
        //減速しようとした時敵が既に凍りついていたら、速度は0にします。
        if (isFreezing == false)
        {
            float DeceleSpeed = originalMoveSpeed * DecelerationRatio;
            moveSpeed = DeceleSpeed;
        }
        else if(isFreezing == true) moveSpeed = 0;
        Invoke("OriginalMoveSpeed", 0.5f);
    }

    //氷に当たって凍結する。
    public void Freeze(float freezeTime)
    {
        isFreezing = true;
        moveSpeed = 0;
        Invoke("FreezeFinish", freezeTime);

        iceAudio.Play();
    }

    //元の移動速度に回復する。
    private void OriginalMoveSpeed()
    {
        moveSpeed = originalMoveSpeed;
    }

    //解凍。
    private void FreezeFinish()
    {
        isFreezing = false;
        moveSpeed = originalMoveSpeed;
    }

    //HPはゼロになる時死亡。
    void Die()
    {
        moveSpeed = 0f;
        animator.SetBool("Death", true);
        canvas.SetActive(false);

        GameObject get = Instantiate(getMoneyText, transform.position, Quaternion.identity);
        Destroy(get, 1f);
        Destroy(gameObject,1f);

        BuildManager.instance.changMoney(getMoney);
        
        deathAudio.Play();
    }
}
