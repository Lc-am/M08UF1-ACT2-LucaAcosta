using DG.Tweening;
using System.Threading;
using UnityEngine;

public class GroundEnemyController : MonoBehaviour
{
    [SerializeField] public float velocity = 5f;
    [SerializeField] public float verticalVelocityOnGrounded = -1f;
    float gravity = -9.8f;
    float verticalVelocity = 0f;
    [SerializeField] float timer = 0;

    CharacterController enemyController;
    
    [SerializeField] HitCollider hitCollider;
    HurtCollider hurtCollider;
    [SerializeField] PlayerDetected playerDetected;
    [SerializeField] int lives = 3;

    private void Awake()
    {
        enemyController = GetComponent<CharacterController>();
        hurtCollider = GetComponent<HurtCollider>();
    }

    private void OnEnable()
    {
        hurtCollider.onHitReceived.AddListener(GetHurt);
    }

    void Update()
    {
        enemyController.Move((-Vector3.forward * velocity + Vector3.up * verticalVelocity) * Time.deltaTime);

        if (enemyController.isGrounded)
        {
            verticalVelocity = verticalVelocityOnGrounded;
        }

        verticalVelocity += gravity * Time.deltaTime;

        if(playerDetected.playerDetector)
        {
            velocity = 0;

            timer += Time.deltaTime;

            if (timer >= 2.5)
            {
                hitCollider.gameObject.SetActive(true);

                DOVirtual.DelayedCall(0.5f,
                () => hitCollider.gameObject.SetActive(false));
                timer = 0;
            }
        }
    }

    private void OnDisable()
    {
        hurtCollider.onHitReceived.RemoveListener(GetHurt);
    }

    private void GetHurt(HitCollider hit, HurtCollider hurt)
    {
        lives--;

        if(lives > 0)
        {

        }
        else
        {
            hitCollider.gameObject.SetActive(false);
            hurtCollider.gameObject.SetActive(false);
            verticalVelocity = 4;
        }
    }
}
