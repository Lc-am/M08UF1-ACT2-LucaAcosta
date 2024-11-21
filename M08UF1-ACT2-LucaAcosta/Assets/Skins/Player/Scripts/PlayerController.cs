using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento de Avance")]
    [SerializeField] public float velocidadPlayer; // Vfwd
    [SerializeField] public float incrementoVelocidad; //dV
    [SerializeField] public float velocidadMaxima; 
    [SerializeField] public float velocidadMinima;

    [Header("Movimiento vertical")]
    [SerializeField] public float velocidadVertical; //Vvert
    [SerializeField] public float gravedad = -9.81f;
    
    public bool isGrounded;

    private Rigidbody rb;

    void Start()
    {
        isGrounded = false;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleForwardMovement();
        HandleVerticalMovement();
    }

    private void HandleForwardMovement()
    {
        if (velocidadPlayer < velocidadMinima)
        {
            velocidadPlayer = velocidadMinima;
        }

        Vector3 forwardMovement = transform.forward * velocidadPlayer * Time.deltaTime;
        rb.MovePosition(rb.position + forwardMovement);
    }

    public void IncreaseForwardSpeed()
    {
        velocidadPlayer = Mathf.Min(velocidadPlayer + incrementoVelocidad, velocidadMaxima);
    }

    public void HalveForwardSpeed()
    {
        velocidadPlayer /= 2f;
    }

    private void HandleVerticalMovement()
    {
        if (!isGrounded)
        {
            velocidadVertical += gravedad * Time.deltaTime;
        }

        Vector3 verticalMovement = new Vector3(0, velocidadVertical * Time.deltaTime, 0);
        rb.MovePosition(rb.position + verticalMovement);
    }

    public void SetVerticalSpeed(float newVerticalSpeed)
    {
        velocidadVertical = newVerticalSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].point.y <= transform.position.y)
        {
            isGrounded = true;
            velocidadVertical = 0f; 
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

}
