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

    IEnumerator FadeAndLoad()
    {
        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(delayBeforeLoad);

        
        SceneManager.LoadScene(GameManager.Instance.nextSceneName);
    }

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

    /*
    IEnumerator TransitionRespawn()
    {
        respawnScreen.SetActive(true); //black screen is enabled
        //fade in black screen
        while (respawnScreen.GetComponentInChildren<CanvasGroup>().alpha < 1)
        {
            respawnScreen.GetComponentInChildren<CanvasGroup>().alpha += Time.deltaTime * 1.5f;
            yield return null;
        }
        yield return new WaitForSeconds(1f); //pause
        player.transform.position = currentCheckpoint.position; //move player to last checkpoint
        //fade out black screen
        while (respawnScreen.GetComponentInChildren<CanvasGroup>().alpha > 0)
        {
            respawnScreen.GetComponentInChildren<CanvasGroup>().alpha -= Time.deltaTime * 1.5f;
            yield return null;
        }

        GameManager.Instance.UpdateGameState(GameManager.GameState.Playing); //allow player input again
    }
    */
}
