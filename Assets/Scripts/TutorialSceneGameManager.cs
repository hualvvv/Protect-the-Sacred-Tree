using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialSceneGameManager : MonoBehaviour
{
    //すべてのページを取得し、動画を取得する。
    public GameObject[] page;
    public int pageNumber = 0;
    public Animator animator;
    //シーン遷移時のFadeに関するものを取得。
    public SceneFader sceneFader;
    public GameObject fade;
    //ButtonAudio。
    public AudioSource buttonAudio;

    private void Start()
    {
        Cursor.visible = true;
        fade.SetActive(true);
        //デフォルトで最初のページを開いて、他のページを閉じます。
        for (int i=1;i<page.Length; i++)
        {
            page[i].SetActive(false);
        }
        page[0].SetActive(true);
    }

    //次のページに遷移。
    public void NextButtonDown()
    {
        if (pageNumber < page.Length - 1 && page[pageNumber].activeSelf == true)
        {
            animator.SetTrigger("Flash");
            page[pageNumber].SetActive(false);
            page[pageNumber + 1].SetActive(true);
            pageNumber++;

            buttonAudio.Play();
        }
        else sceneFader.FadeTo("MainScene");
        buttonAudio.Play();
    }


    //正式にゲームを開始する画面に遷移。
    public void Skip()
    {
        sceneFader.FadeTo("MainScene");
        buttonAudio.Play();
    }
}
