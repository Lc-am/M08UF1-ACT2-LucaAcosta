using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

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

    float forwardVelocity = 0f;
    float verticalVelocity = 0f;
    float gravity = -9.8f;

    CharacterController characterController;
    
    private int triggersTouched = 0;

    private Animator animator;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        punch.action.Enable();
        punch.action.performed += OnPunch;

        uppercut.action.Enable();
        uppercut.action.performed += OnUppercut;

        smash.action.Enable();
        smash.action.performed += OnSmash;
    }
    private void Update()
    {
        if(forwardVelocity < minForwardVelocity)
        {
            forwardVelocity += forwardAcceleration * Time.deltaTime;
        }

        if (forwardVelocity > maxForwardVelocity)
        { 
            forwardVelocity = maxForwardVelocity; 
        }

        characterController.Move((Vector3.forward * forwardVelocity + Vector3.up * verticalVelocity) * Time.deltaTime);

        if(characterController.isGrounded)
        {
            verticalVelocity = verticalVelocityOnGrounded;
        }

        verticalVelocity += gravity * Time.deltaTime;
    }

    private void OnDisable()
    {
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
            verticalVelocity = 1f; // Velocida positiva pero no me hace nada, no se hace la accion de subir un poco
            DOVirtual.DelayedCall(0.5f,
                () => hitColliderUppercut.gameObject.SetActive(false));
        }
    }

    private void OnSmash(InputAction.CallbackContext callbackContext)
    {
        if(!characterController.isGrounded)
        {
            animator.SetTrigger("Smash");
            hitColliderSmash.gameObject.SetActive(true);
            forwardVelocity *= forwardVelocityDecrement;
            verticalVelocity = 1f; // Velocida positiva pero no me hace nada, no se hace la accion de subir un poco
            DOVirtual.DelayedCall(0.5f,
                () => hitColliderSmash.gameObject.SetActive(false));
        }
    }

    // Para generacion de mapa
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ScenarioPieceEnd"))
        {
            triggersTouched++;

            if (triggersTouched == 2)
            {
                ScenarioGenerator scenarioGenerator = ScenarioGenerator.instance;
                scenarioGenerator.EndOfPieceReached();
                triggersTouched = 0;
            }
        }
    }
}
