using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetSceneController : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    [SerializeField] float fadeDuration = 1f;
    [SerializeField] float delayBeforeLoad = 0.5f;

    private void Start()
    {
        StartCoroutine(FadeAndLoad());
    }

    //coroutine that fades in the black image and then loads the next scene after a set amount of time
    IEnumerator FadeAndLoad()
    {
        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(delayBeforeLoad);

        
        SceneManager.LoadScene(GameManager.Instance.nextSceneName); //loads the next scene which is stored in the GameManager
    }

    //coruotine that fades in the black image
    IEnumerator FadeIn()
    {
        float t = 0;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
    }
}
