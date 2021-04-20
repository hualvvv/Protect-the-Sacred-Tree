using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fallen : MonoBehaviour
{
    //プレイヤーが死亡するとカウントダウンして復活する。
    public GameObject countDown;
    public GameObject player;
    //タイマーを取得。
    public float cantMoveTime = 3f;
    private float timer;
    public Text text;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //死亡動画を再生する。２秒後再生が終わりましたら、継続します。
            player.GetComponent<PlayerController>().Death();
            Invoke("FallenAndReset", 2f);

            GetComponent<AudioSource>().Play();
        }
    }

    void FallenAndReset()
    {
        //プレーヤーのFallenAndRest関数をトリガ。
        player.GetComponent<PlayerController>().FallenAndReset();
        //カウントダウンを表示。
        countDown.SetActive(true);
        timer+=cantMoveTime;
        //カウントダウンが終わったら、プレーヤーのcharacterControllerコンポーネントを再起動。
        Invoke("ControllerEnabled", cantMoveTime);
    }

    private void Update()
    {
        //時間計算は小数点以下の桁もでに表示される。
        text.text = timer.ToString("f1");
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else timer = 0;
    }

    void ControllerEnabled()
    {
        //キャラクターのcharacterControllerコンポーネントを再起動。
        player.GetComponent<CharacterController>().enabled = true;
        //カウントダウン表示をオフにする。
        countDown.SetActive(false);

        player.GetComponent<PlayerController>().canShoot = true;
    }
}
