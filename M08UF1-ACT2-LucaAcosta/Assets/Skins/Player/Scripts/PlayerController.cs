using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float forwardAcceleration = 1f;
    [SerializeField] public float maxForwardVelocity = 10f;
    [SerializeField] public float minForwardVelocity = 10f;
    [SerializeField] public float verticalVelocityOnGrounded = -1f;

    [Header("Puñetazo")]
    [SerializeField] InputActionReference punch;
    [SerializeField] HitCollider hitColliderPunch;

    [Header("Uppercut")]
    [SerializeField] InputActionReference uppercut;
    [SerializeField] HitCollider hitColliderUppercut;
    float forwardVelocityDecrement = 0.5f;

    [Header("Smash")]
    [SerializeField] InputActionReference smash;
    [SerializeField] HitCollider hitColliderSmash;

    [Header("Muerte")]
    [SerializeField] public int lives = 3; 
    [SerializeField] private float hitForwardVelocity = -5f; 
    [SerializeField] private float hitVerticalVelocity = 2f; // Velocidad de salto al recibir daño
    private bool isInvulnerable = false;

    float forwardVelocity = 0f;
    float verticalVelocity = 0f;
    float gravity = -9.8f;

    CharacterController characterController;

    private int triggersTouched = 0;

    private Animator animator;

    HurtCollider hurtCollider;

    [SerializeField] float timerInvulnerable = 3;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        hurtCollider = GetComponent<HurtCollider>();
    }
    private void OnEnable()
    {
        hurtCollider.onHitReceived.AddListener(OnHitReceived);

        punch.action.Enable();
        punch.action.performed += OnPunch;

        uppercut.action.Enable();
        uppercut.action.performed += OnUppercut;

        smash.action.Enable();
        smash.action.performed += OnSmash;
    }
    private void Update()
    {
        if (forwardVelocity < minForwardVelocity)
        {
            forwardVelocity += forwardAcceleration * Time.deltaTime;
        }

        if (forwardVelocity > maxForwardVelocity)
        {
            forwardVelocity = maxForwardVelocity;
        }

        characterController.Move((Vector3.forward * forwardVelocity + Vector3.up * verticalVelocity) * Time.deltaTime);

        if (characterController.isGrounded)
        {
            verticalVelocity = verticalVelocityOnGrounded;
        }

        verticalVelocity += gravity * Time.deltaTime;

        if (isInvulnerable)
        {
           timerInvulnerable -= Time.deltaTime;

            if(timerInvulnerable < 0)
            {
                isInvulnerable = false;
            }
        }
    }

    private void OnDisable()
    {
        hurtCollider.onHitReceived.RemoveListener(OnHitReceived);

        punch.action.Disable();
        punch.action.performed -= OnPunch;

        uppercut.action.Disable();
        uppercut.action.performed -= OnUppercut;

        smash.action.Disable();
        smash.action.performed -= OnSmash;
    }

    private void OnPunch(InputAction.CallbackContext callbackContext)
    {
        animator.SetTrigger("Punch");

        hitColliderPunch.gameObject.SetActive(true);
        DOVirtual.DelayedCall(0.5f,
            () => hitColliderPunch.gameObject.SetActive(false));
    }

    private void OnUppercut(InputAction.CallbackContext callbackContext)
    {

        if (characterController.isGrounded)
        {
            animator.SetTrigger("Uppercut");
            hitColliderUppercut.gameObject.SetActive(true);
            forwardVelocity *= forwardVelocityDecrement;
            verticalVelocity = 3f;  
            DOVirtual.DelayedCall(0.5f,
                () => hitColliderUppercut.gameObject.SetActive(false));
        }
    }

    private void OnSmash(InputAction.CallbackContext callbackContext)
    {
        if (!characterController.isGrounded)
        {
            animator.SetTrigger("Smash");
            hitColliderSmash.gameObject.SetActive(true);
            forwardVelocity *= forwardVelocityDecrement;
            verticalVelocity = -4f; 
            DOVirtual.DelayedCall(0.5f,
                () => hitColliderSmash.gameObject.SetActive(false));
        }
    }

    // Para generacion de mapa
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ScenarioPieceEnd"))
        {
            triggersTouched++;

            if (triggersTouched == 2)
            {
                ScenarioGenerator.instance.EndOfPieceReached();
                triggersTouched = 0;
            }
        }
    }

    ////Nuevo
    private void OnHitReceived(HitCollider hit, HurtCollider hurt)
    {   
        if(isInvulnerable)
        {
            return;
        }
        
        
        if(lives != 0)
        {
            lives--;
            verticalVelocity = 3;
            forwardVelocity = -2;

            isInvulnerable = true;

            Debug.Log(lives);
        }
        else
        {
            forwardVelocity = 0;
            isInvulnerable = true;
            animator.SetTrigger("IsDead");
            DOVirtual.DelayedCall(5f, RestartScene);
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
