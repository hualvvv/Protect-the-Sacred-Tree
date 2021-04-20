using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //移動スビート、重力、跳躍の高さなどパラメータを設定する。
    public float moveSpeed = 6.0f;
    public float gravity = 8.0f;
    public float jumpHeight =10.0f;
    //CharacterControllerを取得。
    private CharacterController characterController;
    //各方向への動き。
    private Vector3 movementDirection;
    //获得动画组件.
    private Animator Animator;
    //スキルを使えるのか。
    private bool useSkill=false;
    //弾を取得。
    public GameObject playerBulletPrefab;
    //弾のスビートを設定。
    public float bulletSpeed = 20f;
    //弾を発射するの位置。
    public GameObject firePosition;
    //撃退されたかどうか。撃退の距離。撃退された状態の持続時間。撃退するときのエフェクト。
    [HideInInspector]
    public bool repelled=false;
    private float repelledTime = 0.5f;
    private float repelldeDistance = 50f;
    public GameObject repelledEffect;
    //プレーヤーが死亡する時の蘇生位置。
    public GameObject defaultPosition;
    //移動できますか。
    private bool canMove=true;
    public bool canShoot = true;
    //敵とぶつかる時、敵とプレーヤーの間のオフセット。
    private Vector3 offset;



    void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
    }


    void Update()
    {
        if (characterController.enabled == true)
        {
            movement();
        }
        void movement()
        {
            //地面に立っている時，HorizontalとVerticalを獲得します.
            if (characterController.isGrounded)
            {
                if (canMove == true)
                {
                    float m_Horizontal = Input.GetAxis("Horizontal");
                    float m_Vertical = Input.GetAxis("Vertical");
                    movementDirection = transform.TransformDirection(new Vector3(m_Horizontal, 0, m_Vertical));
                    //移動する時、相応なアニメーションを流れる。
                    if (m_Horizontal != 0 || m_Vertical != 0)
                    {
                        Animator.SetFloat("Speed", moveSpeed);
                    }
                    else Animator.SetFloat("Speed", 0);
                }
                else movementDirection = transform.TransformDirection(new Vector3(0, 0, 0));

                //キャラクターの跳躍.
                if (Input.GetKey(KeyCode.Space))
                {
                    movementDirection.y = jumpHeight;
                }
            }
            //キャラクターの重力.
            movementDirection.y -= gravity * Time.deltaTime;
            //キャラクターの移動.
            characterController.Move(movementDirection * moveSpeed * Time.deltaTime);
        }

        //速度の変更.
        if (Input.GetKeyDown(KeyCode.LeftShift) && useSkill == false)
        {
            moveSpeed = 10.0f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && useSkill == false)
        {
            moveSpeed = 6.0f;
        }

        //左バタンーのスキルの使用。
        if (Input.GetMouseButtonDown(0)&& useSkill == false && canShoot == true)
        {
            PlayerBullet bullet = playerBulletPrefab.GetComponent<PlayerBullet>();
            bullet.damageType = PlayerBullet.E_DamageType.Fire;
            bullet.fireShoot.SetActive(true);
            bullet.iceShoot.SetActive(false);

            UseSkill();
        }

        //右バタンーのスキルの使用。
        if (Input.GetMouseButtonDown(1) && useSkill == false && canShoot == true)
        {
            PlayerBullet bullet = playerBulletPrefab.GetComponent<PlayerBullet>();
            bullet.damageType = PlayerBullet.E_DamageType.ICE;
            bullet.fireShoot.SetActive(false);
            bullet.iceShoot.SetActive(true);

            UseSkill();
        }

        //撃退さらたとき。
        if (repelled == true)
        {
            characterController.enabled = false;
            //撃退距離はOffset.normalized*repelldeDistance*time.deltatime。XとZ方向で撃退する。
            float deltDistance = repelldeDistance* Time.deltaTime;
            transform.Translate(offset.x* deltDistance, 0,offset.z * deltDistance, Space.World);

            Invoke("RepelledOver", repelledTime);
        }
    }

    void UseSkill()
    {
        useSkill = true;
        //マウスが呼び出されていない場合、アニメーションに合わせるため、1秒後起動する。
        if (Cursor.visible == false)
        {
            Invoke("Shoot", 0.9f);
            Animator.SetTrigger("Skill");
            Invoke("useSkillOver", 1f);
            //シューティングする時は移動できない。
        }
        else Invoke("useSkillOver", 0f);
        canMove=false;
        
    }

    void useSkillOver()
    {
        useSkill = false;
        canMove = true;
    }

    void Shoot()
    {
        //メインカメラの前方にRayを発射し、方向を判定する。
        Ray ray = Camera.main.ScreenPointToRay(Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject playerBullet = Instantiate(playerBulletPrefab, firePosition.transform.position, Quaternion.identity);
            playerBullet.transform.LookAt(hit.point);
            Rigidbody bulletRigid = playerBullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = Camera.main.transform.forward * bulletSpeed;
            Destroy(playerBullet, 3f);
        }
    }

    //撃退行為終了。
    void RepelledOver()
    {
        characterController.enabled = true;
        repelled = false;
    }

    //敵とぶつかると、撃退行為がトリガーされる。
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            repelled = true;
            Vector3 collider=transform.GetComponent<CapsuleCollider>().ClosestPointOnBounds(transform.position);
            GameObject effect = Instantiate(repelledEffect,collider,Quaternion.identity);
            Destroy(effect, 1f);

            GetComponent<AudioSource>().Play();

            //敵とぶつかる時、敵とプレーヤーの間のオフセット。
            offset = (transform.position - other.transform.position).normalized;
        }
    }

    //プラットフォームから落ちたら.位置をリセットします。
    public void FallenAndReset()
    {
        //キャラクターの位置を復活位置に調整する。
        transform.position = defaultPosition.transform.position;
        GetComponentInChildren<Animator>().SetFloat("Speed", 0);
    }

    //プレーヤー死亡。
    public void Death()
    {
        canShoot = false;
        //死亡動画とエフェクト。
        GetComponentInChildren<Animator>().SetTrigger("Death");
        GameObject effect =Instantiate(repelledEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1f);

        //characterControllerを停止する。
        characterController.enabled = false;
    }
}
