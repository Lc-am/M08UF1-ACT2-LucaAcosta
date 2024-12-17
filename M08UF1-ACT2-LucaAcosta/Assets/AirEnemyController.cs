//using UnityEngine;

//public class AirEnemyController : MonoBehaviour
//{
//    [SerializeField] private float speed = 5f;  // Velocidad del enemigo.
//    [SerializeField] private float fleeSpeed = 7f;  // Velocidad de huida.
//    [SerializeField] private float fleeDuration = 2f;  // Duraci�n de la huida.
//    private Vector3 moveDirection = Vector3.back;  // Direcci�n inicial hacia la izquierda (-Z).

//    private CharacterController characterController;
//    private bool isFleeing = false;
//    private float fleeTimer = 0f;

//    private void Awake()
//    {
//        characterController = GetComponent<CharacterController>();
//    }

//    private void Update()
//    {
//        // Si est� huyendo, manejar el comportamiento de huida.
//        if (isFleeing)
//        {
//            fleeTimer += Time.deltaTime;

//            if (fleeTimer >= fleeDuration)
//            {
//                // Deja de huir despu�s de un tiempo.
//                isFleeing = false;
//                fleeTimer = 0f;
//                moveDirection = Vector3.back;  // Vuelve a la direcci�n original (-Z).
//            }
//            else
//            {
//                // Huir en la direcci�n positiva de Z y Y.
//                characterController.Move((Vector3.forward + Vector3.up) * fleeSpeed * Time.deltaTime);
//            }
//        }
//        else
//        {
//            // Mover hacia la izquierda normalmente.
//            characterController.Move(moveDirection * speed * Time.deltaTime);
//        }

//    }

//    private void OnControllerColliderHit(ControllerColliderHit hit)
//    {
//        // Si el enemigo choca con algo, comienza a huir.
//        if (!isFleeing)
//        {
//            // Puedes verificar si el choque es con un enemigo u otro objeto espec�fico,
//            // pero por ahora huye siempre que haya cualquier colisi�n.
//            isFleeing = true;

//            // Cambia la direcci�n a +Z y +Y para huir.
//            moveDirection = Vector3.forward + Vector3.up;
//        }
//    }
//}

using UnityEngine;

public class AirEnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;  // Velocidad del enemigo.
    [SerializeField] private float fleeSpeed = 7f;  // Velocidad de huida.
    [SerializeField] private float fleeDuration = 2f;  // Duraci�n de la huida.
    [SerializeField] private float attackRange = 10f;  // Rango de ataque.
    [SerializeField] private float attackDuration = 5f;  // Duraci�n de la persecuci�n al jugador.
    [SerializeField] private float attackAngle = 45f;  // �ngulo de ataque.
    private Vector3 moveDirection = Vector3.back;  // Direcci�n inicial hacia la izquierda (-Z).

    private CharacterController characterController;
    private bool isFleeing = false;
    private bool isAttacking = false;
    private float fleeTimer = 0f;
    private float attackTimer = 0f;

    private Transform player;
    [SerializeField] private HitCollider hitCollider;
    private bool hasDamagedPlayer = false;  // Para evitar da�ar al jugador m�s de una vez durante un ataque.

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        player = GameObject.FindWithTag("Player").transform;  // Encuentra al jugador por su etiqueta (aseg�rate de que el jugador tenga la etiqueta "Player").
    }

    private void Update()
    {
        if (isAttacking)
        {
            AttackBehavior();
        }
        else if (isFleeing)
        {
            FleeBehavior();
        }
        else
        {
            // Si no est� huyendo ni atacando, se mueve normalmente
            characterController.Move(moveDirection * speed * Time.deltaTime);
        }

        // Detectar si el jugador est� dentro del rango de ataque
        if (Vector3.Distance(transform.position, player.position) <= attackRange && !isAttacking)
        {
            // Empieza a perseguir al jugador
            isAttacking = true;
            attackTimer = attackDuration;
        }
    }

    private void AttackBehavior()
    {
        // Durante 5 segundos, intenta alcanzar al jugador
        attackTimer -= Time.deltaTime;

        if (attackTimer > 0f)
        {
            // Mover al enemigo hacia el jugador en l�nea recta
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            directionToPlayer.y = 0;  // Asegurarse de que no haya movimiento en el eje Y
            characterController.Move(directionToPlayer * speed * Time.deltaTime);

            // Rotar hacia el jugador para un ataque en l�nea recta
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

            // Comprobar si el HitCollider da�a al jugador
            if (!hasDamagedPlayer && Vector3.Distance(transform.position, player.position) <= 1f)
            {
                hitCollider.gameObject.SetActive(true);
                hasDamagedPlayer = true;
                // Despu�s de un peque�o retraso, desactivar el HitCollider
                Invoke("ResetHitCollider", 0.5f);
            }
        }
        else
        {
            // Si ha pasado el tiempo de ataque, empieza a huir
            isAttacking = false;
            isFleeing = true;
            fleeTimer = fleeDuration;
            moveDirection = Vector3.forward + Vector3.up;  // Direcci�n de huida
        }
    }

    private void FleeBehavior()
    {
        fleeTimer -= Time.deltaTime;

        if (fleeTimer > 0f)
        {
            // Huir en la direcci�n positiva de Z y Y
            characterController.Move((Vector3.forward + Vector3.up) * fleeSpeed * Time.deltaTime);
        }
        else
        {
            // Termina la huida y vuelve a la direcci�n original
            isFleeing = false;
            moveDirection = Vector3.back;  // Vuelve a moverse hacia la izquierda
        }
    }

    private void ResetHitCollider()
    {
        hitCollider.gameObject.SetActive(false);
        hasDamagedPlayer = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Si el enemigo choca con algo que no sea el jugador mientras huye, no cambia su comportamiento
    }
}
