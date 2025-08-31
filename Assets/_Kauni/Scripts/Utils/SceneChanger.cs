using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Image background;

    private void Awake()
    {
        StartCoroutine(FadeInScene());
    }

    IEnumerator FadeInScene()
    {
        background.DOFade(0, 1);
        yield return new WaitForSeconds(5f);
        StartCoroutine(FadeOutScene());
    }

    private IEnumerator FadeOutScene()
    {
        background.DOFade(1, 1);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("MainMenu");
    }
}
