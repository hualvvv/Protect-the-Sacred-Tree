using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    //Fade用の画像。Fadeの速度。
    public Image image;
    public float speed=2f;
    //新しいシーンに入るとまずはFadeIn。
    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeTo(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    //透明から黒に変わる。
    IEnumerator FadeIn()
    {
        image.enabled = true;
        float alpha = 1f;

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime*speed;
            Color mColor = image.color;
            image.color = new Color (mColor.r, mColor.g,mColor.b,alpha);
            yield return 0;
        }
        image.enabled = false;
    }

    //黒から透明に変わる。
    IEnumerator FadeOut(string sceneName)
    {
        image.enabled = true;
        float alpha = 0f;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * speed;
            Color mColor = image.color;
            image.color = new Color(mColor.r, mColor.g, mColor.b, alpha);
            yield return 0;
        }
        SceneManager.LoadScene(sceneName);
    }
}
