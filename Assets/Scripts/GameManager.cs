using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    [DllImport("user32.dll")]
    public static extern int SetCursorPos(int x, int y);
    private int frameRate=90;
    //ゲームUIを取得。
    public GameObject EopenUI;
    public GameObject turretSwitchUI;
    public GameObject upgradeRemoveUI;
    public GameObject removeUI;
    public Canvas canvas;
    public GameObject promptText;
    public GameObject CountdownUI;
    public GameObject ifReady;
    public GameObject redPoint;
    public GameObject endUI;
    public Text endMessage;
    public GameObject pauseUI;
    //EnemySpawnerを取得。
    private EnemySpawner enemySpawner;
    //一時停止しましたか。停止バタンーを押せますか。
    private bool pausing=false;
    private bool canUsePause = true;
    //シーン遷移時のFadeに関するものを取得。
    public SceneFader sceneFader;
    public GameObject fade;
    //自身のInstanceをつくる。
    public static GameManager instance;
    //ButtonAudio。
    public AudioSource buttonAudio;
    //Audio。
    public AudioClip[] audioClip;


    private void Awake()
    {
        Application.targetFrameRate = frameRate;
    }

    void Start()
    {
        instance = this;
        enemySpawner = GetComponent<EnemySpawner>();

        //Cursorを隠す.
        Cursor.visible = false;
        //"Eで砲塔操作するUIを閉じる。
        EopenUI.SetActive(false);
        //建設する砲塔を選択するUIを閉じる。
        turretSwitchUI.SetActive(false);
        //砲塔のUpgradeUIを閉じる。
        upgradeRemoveUI.SetActive(false);
        //砲塔のRemoveUIを閉じる。
        removeUI.SetActive(false);
        //新しい敵が来るのヒントUIを閉じる。
        promptText.SetActive(true);
        //カウントダウンUIを閉じる。
        CountdownUI.SetActive(false);
        //ゲームが始まる時のヒントUIを閉じる。
        ifReady.SetActive(true);
        //ゲームが終わる時のヒントUIを閉じる。
        endUI.SetActive(false);
        //スクリーン中央の赤点UIを閉じる。
        redPoint.SetActive(true);
        //一時停止UIを閉じる。
        pauseUI.SetActive(false);
        //FadeUIを開く。
        fade.SetActive(true);
    }

    void Update()
    {
        //左のALTを押し、Cursorの表示と非表示を制御します。
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.visible = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.visible = false;
        }

        //Cursorは非表示の状態でしたら、Cursorは常にスクリーンの中心に位置します。
        KeepCursorCenter();

        //Escapeを押すと、一時停止します。もう一度押し続けます。
        //または画面上のContinueボタンをクリックして続行します。
        if (Input.GetKeyDown(KeyCode.Escape) && canUsePause == true)
        {
            if (pausing == false)
            {
                Cursor.visible = true;
                canUsePause = false;
                pauseUI.SetActive(true);
                pauseUI.GetComponent<Animator>().SetBool("Show", true);
                buttonAudio.Play();

                //pauseUIのAnimatorコンポーネントを取得して、updateModeをUnscaledTimeモードに変更する。
                Animator[] anim = pauseUI.GetComponentsInChildren<Animator>();
                for(int i=0; i < anim.Length; i++)
                {
                    anim[i].updateMode = AnimatorUpdateMode.UnscaledTime;
                }
                
                Invoke("Pause", 0.5f);
                Invoke("CanUsePause", 0.5f);
            }

            else
            {
                //ゲーム続く。
                Continue();
            }
        }
    }

    //0.5秒後一時停止.
    void Pause()
    {
        pausing = true;
        Time.timeScale = 0;
    }
    void CanUsePause()
    {
        canUsePause = true;  
    }

    //Cursorは非表示の状態でしたら、Cursorは常にスクリーンの中心に位置します。
    private void KeepCursorCenter()
    {
        if (Cursor.visible == false)
        {
            SetCursorPos((int)Screen.width / 2, (int)Screen.height / 2);
        }
    }



    //ゲームに勝った時や失敗した時。
    public void Win()
    {
        endUI.SetActive(true);
        endMessage.text = "WIN";
        enemySpawner.StopSpawn();
        Cursor.visible = true;
        redPoint.SetActive(false);

        GetComponent<AudioSource>().clip = audioClip[0];
        GetComponent<AudioSource>().Play();
    }
    public void Defeat()
    {
        endUI.SetActive(true);
        endMessage.text = "GAME OVER";
        enemySpawner.StopSpawn();
        Cursor.visible = true;
        redPoint.SetActive(false);

        GetComponent<AudioSource>().clip = audioClip[1];
        //他のEnemyを全部削除。
        GetComponent<AudioSource>().Play();
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i=0; i < enemy.Length; i++)
        {
            Destroy(enemy[i]);
        }
    }

    //一時停止されたゲームが継続された時。
    public void Continue()
    {
        Cursor.visible = false;
        canUsePause = false;
        pauseUI.GetComponent<Animator>().SetBool("Show", false);
        pausing = false;
        Time.timeScale = 1;

        //pauseUIのAnimatorコンポーネントを取得して、updateModeをnormalモードに変更する。
        Animator[] anim = pauseUI.GetComponentsInChildren<Animator>();
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].updateMode = AnimatorUpdateMode.Normal;
        }

        buttonAudio.Play();

        Invoke("CanUsePause", 0.5f);
        Invoke("ActiveFalse", 0.5f);
    }

    void ActiveFalse()
    {
        pauseUI.SetActive(false);
    }

    //continue、replayまたはmenuボタンを押した時。
    public void Replay()
    {
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
        buttonAudio.Play();
    }

    public void OnButtonMenu()
    {
        sceneFader.FadeTo("MenuScene");
        buttonAudio.Play();
    }
}
