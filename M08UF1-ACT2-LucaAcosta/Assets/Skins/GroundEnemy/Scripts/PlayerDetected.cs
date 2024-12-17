using UnityEngine;

public class PlayerDetected : MonoBehaviour
{
    public bool playerDetector = false;

    private void Start()
    {
        playerDetector = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerDetector = true;
        }
    }
}
