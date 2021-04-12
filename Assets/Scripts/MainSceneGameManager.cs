using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneGameManager : MonoBehaviour
{
    //シーン遷移時のFadeに関するものを取得。
    public SceneFader SceneFader;
    public GameObject fade;
    //BGM、シーン遷移時保留。
    public GameObject BGM;
    public AudioSource buttonAudio;

    private void Start()
    {
        Cursor.visible = true;
        fade.SetActive(true);
        DontDestroyOnLoad(BGM);
    }
    //ゲームの開始と終了をコントロール。
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void StartGame()
    {
        SceneFader.FadeTo("TutorialScene");
        buttonAudio.Play();
    }

    public void Exit()
    {
        Application.Quit();
    }

}
