using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;


public class LoadingScene : MonoBehaviour
{
    [SerializeField] CanvasGroup fader;
    Scene currentScene;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    static public LoadingScene instance;

    private void Awake()
    {
        instance = this;
    }

    public IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // Fade
        {
            Tween fadeTween = fader.DOFade(1f, 1f);
            do
            {
                yield return new();
            }
            while (fadeTween.IsPlaying());
        }

        // Descargar la escena actual
        if (currentScene.isLoaded)
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(currentScene);
            do
            {
                yield return new();
            }
            while (!unloadOperation.isDone);
        }

        //Para que tenga un Tiempo Determinado
        float timeBeforeLoad = Time.realtimeSinceStartup;

        //Cargar la escena
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            do
            {
                yield return new();
            }
            while (!loadOperation.isDone);

            currentScene = SceneManager.GetSceneAt(1);
            SceneManager.SetActiveScene(currentScene);
        }

        //Continuacion Tiempo Determinado
        float timeElapsedLoading = Time.realtimeSinceStartup - timeBeforeLoad;
        if(timeElapsedLoading < 3f)
        {
            yield return new WaitForSeconds(3f -  timeElapsedLoading);
        }

        //Fade
        {
            Tween fadeTween = fader.DOFade(0f, 1f);
            do
            {
                yield return new();
            }
            while (fadeTween.IsPlaying());
        }
    }

    [MenuItem("LoadingScene/Debug/Change to GameScene")]
    [MenuItem("LoadingScene/Debug/Change to Ejercicio1Scene")]

    static public void DebugChangeToMainMenuScene()
    {
        LoadingScene.instance.LoadScene("Game");
    }

    static public void DebugChangeToEjercicio1Scene()
    {
        LoadingScene.instance.LoadScene("Ejercicio1");
    }
}
