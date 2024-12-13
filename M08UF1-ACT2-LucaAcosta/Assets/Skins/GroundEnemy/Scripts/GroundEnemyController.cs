using UnityEngine;

public class GroundEnemyController : MonoBehaviour
{
    [SerializeField] public float velocity = 5f;
    [SerializeField] public float verticalVelocityOnGrounded = -1f;
    float gravity = -9.8f;
    float verticalVelocity = 0f;

    CharacterController enemyController;

    private void Awake()
    {
        enemyController = GetComponent<CharacterController>();
    }

    void Update()
    {
        enemyController.Move((-Vector3.forward * velocity + Vector3.up * verticalVelocity) * Time.deltaTime);

        if (enemyController.isGrounded)
        {
            verticalVelocity = verticalVelocityOnGrounded;
        }

        verticalVelocity += gravity * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            velocity = 0;
        }
    }
}
