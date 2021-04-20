using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //生きている敵の数。
    public static int enemyAliveCount = 0;
    //敵の波数。
    public EnemyWave[] waves;
    //現在の波数
    private float currentWave;
    //敵の生成する位置。
    public Transform start;
    //波当たりの敵の数。
    public float waveRate = 3;
    //新しい敵が来るのヒントUI。
    public GameObject promptText;
    //怪を産み出すことができるかどうか。 
    private bool canSpawn = true;
    //ゲームが始まる時のヒントUI。
    public GameObject ifReady;
    //Animatorを取得。
    public Animator animator;
    //BGMをコントロール。
    private GameObject BGM;
    private int changeBgmWave=2;

    private void Start()
    {
        canSpawn = true;
        BGM = GameObject.FindGameObjectWithTag("Sound");
        if(BGM != null&& BGM.GetComponent<AudioSource>().clip != AudioManager.instance.audioClips[0])
        {
            BGM.GetComponent<AudioSource>().clip = AudioManager.instance.audioClips[0];
            BGM.GetComponent<AudioSource>().Play();
        }
    }

    private void Update()
    {
        //Kを押すと、敵が生まれ始めます。
        if (Input.GetKey(KeyCode.K)&& canSpawn == true)
        {
            animator.SetBool("ShowAnim",false);
            canSpawn = false;
            StartCoroutine("SpawnEnemy");

            if (BGM != null)
            {
                //BGM変化。
                BGM.GetComponent<AudioSource>().clip = AudioManager.instance.audioClips[1];
                BGM.GetComponent<AudioSource>().Play();
            }
        }
    }
    public void StopSpawn()
    {
        StopCoroutine("SpawnEnemy");
        BGM.GetComponent<AudioSource>().Stop();
    }

    IEnumerator SpawnEnemy()
    {
        foreach(EnemyWave wave in waves)
        {
            //同じタイプの敵を複数生成する。
            for (int i = 0; i < wave.count; i++)
            {
                Instantiate(wave.enemyPrefab, start.position, Quaternion.identity);
                enemyAliveCount++;
                if (i < wave.count - 1)
                {
                    yield return new WaitForSeconds(wave.rate);
                }            
            }
            while (enemyAliveCount > 0)
            {
                yield return 0;
            }
            yield return new WaitForSeconds(waveRate);
            currentWave++;
            if (currentWave < waves.Length)
            {
                //新しい敵が来るのヒントUI。提示する。
                promptText.GetComponent<Animator>().SetTrigger("Show");
                Invoke("promptActiveFalse", 5f);

                if (currentWave == changeBgmWave&& BGM != null)
                {
                    //BGM変化。
                    BGM.GetComponent<AudioSource>().clip = AudioManager.instance.audioClips[2];
                    BGM.GetComponent<AudioSource>().Play();
                }
            }
        }
        while (enemyAliveCount > 0)
        {
            yield return 0;
        }
        GameManager.instance.Win();
        BGM.GetComponent<AudioSource>().Stop();
    }
}
