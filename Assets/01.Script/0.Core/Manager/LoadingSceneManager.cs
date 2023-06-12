using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    private static int nextScene;

    [SerializeField] private float minimumWaitTime;
    [SerializeField] private AudioClip getLatterClip;

    static bool isLoading = false;

    private float timer = 0.0f;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    public static void LoadScene(int sceneIndex)
    {
        if (isLoading) return;
        //Debug.Log("&&& ·Îµùµù");
        isLoading = true;
        Time.timeScale = 1;
        nextScene = sceneIndex + 1;
        SceneChangeCanvas.Active(() =>
        {
            SceneManager.LoadScene("Loading");
        });
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        while (!op.isDone)
        {
            if (op.progress >= 0.85f && minimumWaitTime < timer)
            {
                AudioManager.PlayAudio(getLatterClip);

                yield return new WaitForSeconds(0.35f);
                isLoading = false;
                op.allowSceneActivation = true;
                SceneChangeCanvas.DeActive();
                yield break;
            }
            yield return null;
        }
    }
}
