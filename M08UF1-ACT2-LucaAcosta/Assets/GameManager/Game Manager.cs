using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverUI; 
    [SerializeField] GameObject deathUI;     
    [SerializeField] Button restartButton;
    [SerializeField] Button quitButton;

    private void Start()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        if (deathUI != null)
        {
            deathUI.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    public void PlayerDied()
    {
        // Ralentizar el tiempo para el efecto dramático
        Time.timeScale = 0.2f;  
        gameOverUI.SetActive(true);
       
        Invoke("ActivateDeathUI", 1.5f); 
    }
    private void ActivateDeathUI()
    {
        if (deathUI != null)
        {
            deathUI.SetActive(true);
        }
        StartCoroutine(HandleDeathCanvas());
    }

    private IEnumerator HandleDeathCanvas()
    {
        while (deathUI.activeSelf)
        {
            yield return null;  // Espera un frame
        }
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void QuitGame()
    {
        Time.timeScale = 1f;

        Application.Quit();
    }
}
