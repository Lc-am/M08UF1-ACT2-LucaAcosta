using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverUI;   // Canvas de Game Over (el primer canvas)
    [SerializeField] GameObject deathUI;      // Canvas con los botones de Reiniciar y Salir (segundo canvas)
    [SerializeField] Button restartButton;
    [SerializeField] Button quitButton;

    private void Start()
    {
        // Inicialmente los Canvas est�n desactivados
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        if (deathUI != null)
        {
            deathUI.SetActive(false);
        }

        // Asignar los botones de reiniciar y salir
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    // Este m�todo es llamado cuando el jugador muere
    public void PlayerDied()
    {
        // Ralentizar el tiempo para el efecto dram�tico
        Time.timeScale = 0.2f;  // Ajusta el valor para hacer el efecto de desaceleraci�n

        // Activar el Canvas de Game Over inmediatamente
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // Llamar a Invoke para activar el Canvas con los botones despu�s de 4 segundos
        Invoke("ActivateDeathUI", 1.5f);  // Llama al m�todo ActivateDeathUI despu�s de 4 segundos
    }

    // M�todo para activar el Canvas con los botones de Reiniciar y Salir
    private void ActivateDeathUI()
    {
        // Activar el Canvas de los botones (no afectado por el timeScale)
        if (deathUI != null)
        {
            deathUI.SetActive(true);
        }

        // Aseg�rate de que el Canvas de botones no se vea afectado por el TimeScale
        StartCoroutine(HandleDeathCanvas());
    }

    // Esta Coroutine se encargar� de actualizar el Canvas de muerte con Time.unscaledDeltaTime
    private IEnumerator HandleDeathCanvas()
    {
        // Realiza las acciones que deber�an ser independientes del Time.timeScale
        // Por ejemplo, si tienes animaciones o alg�n temporizador, usas Time.unscaledDeltaTime
        while (deathUI.activeSelf)
        {
            // Aqu� podr�as actualizar cualquier animaci�n o temporizador en el Canvas de muerte
            // Usamos Time.unscaledDeltaTime para no depender del Time.timeScale

            yield return null;  // Espera un frame
        }
    }

    private void RestartGame()
    {
        // Restaurar la velocidad normal del tiempo
        Time.timeScale = 1f;

        // Recargar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void QuitGame()
    {
        // Restaurar la velocidad normal del tiempo
        Time.timeScale = 1f;

        // Salir de la aplicaci�n
        Application.Quit();
    }
}
