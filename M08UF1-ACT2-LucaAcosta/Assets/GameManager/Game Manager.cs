using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] CinemachineVirtualCamera finishCamera;
    [SerializeField] private GameObject canvas; 

    [SerializeField] private float delayBeforeCanvas = 3f;
    private bool isGameOverTriggered = false;

    private void Update()
    {
        // Comprueba constantemente si el jugador ha muerto
        if (!isGameOverTriggered && player.lives <= 0)
        {
            TriggerGameOver();
            isGameOverTriggered = true;
        }
    }

    private void TriggerGameOver()
    {
        finishCamera.gameObject.SetActive(true);
        StartCoroutine(ActivateGameOverCanvas());
    }
    private IEnumerator ActivateGameOverCanvas()
    {
        yield return new WaitForSeconds(delayBeforeCanvas);

        canvas.SetActive(true);
    }

}
