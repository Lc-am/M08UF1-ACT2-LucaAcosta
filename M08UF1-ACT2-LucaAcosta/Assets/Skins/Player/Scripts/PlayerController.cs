using DG.Tweening;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float forwardAcceleration = 1f;
    [SerializeField] public float maxForwardVelocity = 10f;
    [SerializeField] public float minForwardVelocity = 10f;
    [SerializeField] public float verticalVelocityOnGrounded = -1f;

    [SerializeField] InputActionReference punch;
    [SerializeField] HitCollider hitColliderPunch;

    float forwardVelocity = 0f;
    float verticalVelocity = 0f;
    float gravity = -9.8f;

    CharacterController characterController;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        punch.action.Enable();
        punch.action.performed += OnPunch;
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
    }

    private void OnPunch(InputAction.CallbackContext callbackContext)
    {
        hitColliderPunch.gameObject.SetActive(true);
        DOVirtual.DelayedCall(0.5f, 
            () => hitColliderPunch.gameObject.SetActive(false));
    }
}
