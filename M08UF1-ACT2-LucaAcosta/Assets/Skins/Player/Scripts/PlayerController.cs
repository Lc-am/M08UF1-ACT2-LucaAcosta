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
        // Si la velocidad es menor que Vmin, incrementa hasta Vmin
        if (velocidadPlayer < velocidadMinima)
        {
            velocidadPlayer = velocidadMinima;
        }

        // Aplica la velocidad hacia adelante
        Vector3 forwardMovement = transform.forward * velocidadPlayer * Time.deltaTime;
        rb.MovePosition(rb.position + forwardMovement);
    }

    public void IncreaseForwardSpeed()
    {
        // Incremento fijo de velocidad hasta un máximo
        velocidadPlayer = Mathf.Min(velocidadPlayer + incrementoVelocidad, velocidadMaxima);
    }

    public void HalveForwardSpeed()
    {
        // Reduce la velocidad a la mitad
        velocidadPlayer /= 2f;
    }

    private void HandleVerticalMovement()
    {
        // Aplicar gravedad mientras no esté en el suelo
        if (!isGrounded)
        {
            velocidadVertical += gravedad * Time.deltaTime;
        }

        // Aplica la velocidad vertical
        Vector3 verticalMovement = new Vector3(0, velocidadVertical * Time.deltaTime, 0);
        rb.MovePosition(rb.position + verticalMovement);
    }

    public void SetVerticalSpeed(float newVerticalSpeed)
    {
        // Modifica la velocidad vertical instantáneamente
        velocidadVertical = newVerticalSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Comprueba si toca el suelo
        if (collision.contacts[0].point.y <= transform.position.y)
        {
            isGrounded = true;
            velocidadVertical = 0f; // Reinicia la velocidad vertical al tocar el suelo
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Sale del suelo
        isGrounded = false;
    }

}
