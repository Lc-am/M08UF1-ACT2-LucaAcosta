//using DG.Tweening;
//using UnityEngine;

//public class AirEnemyController : MonoBehaviour
//{
//    // Variables públicas para controlar el movimiento y ataque
//    public float moveSpeed = 5f;
//    public float escapeSpeed = 7f;
//    public float escapeHeight = 5f;
//    public float escapeDuration = 2f;
//    public Transform player;
//    public HitCollider hitCollider;

//    // Estado interno del enemigo
//    private Vector3 moveDirection = Vector3.back;
//    private bool isEscaping = false;
//    private bool isAttacking = false;
//    private bool isFleeingForever = false;
//    private float attackTimer = 0f;
//    private float escapeTimer = 0f;

//    // Configuración de ataque
//    public float attackRange = 10f;
//    public float attackDuration = 5f;
//    public float attackSpeed = 10f;
//    public float extraDetectionRange = 2f;

//    // Componentes para detección de colisiones
//    private HurtCollider hurtCollider;

//    // Configuración de muerte y efectos visuales
//    public float deathForce = 5f;
//    private bool isDead = false;
//    public ParticleSystem hurtParticles;

//    // Método Awake para inicialización temprana
//    void Awake()
//    {
//        // Inicialización del HitCollider
//        if (hitCollider != null)
//        {
//            hitCollider.gameObject.SetActive(false); // Inicialmente desactivado
//            hitCollider.onHitDelivered.AddListener(OnHitDelivered); // Escucha de la colisión
//        }

//        // Inicialización de HurtCollider
//        hurtCollider = GetComponentInChildren<HurtCollider>();
//    }

//    private void OnEnable()
//    {
//        hurtCollider.onHitReceived.AddListener(GetHurtAir);
//    }
//    void Start() { }

//    // Método Update se ejecuta en cada frame
//    void Update()
//    {
//        if (isFleeingForever)
//        {
//            HandleEscape();
//        }
//        else if (isAttacking)
//        {
//            HandleAttack();
//        }
//        else
//        {
//            MoveInDirection();
//            DetectPlayerForAttack();
//        }
//    }

//    private void OnDisable()
//    {
//        hurtCollider.onHitReceived.RemoveListener(GetHurtAir);
//        DOTween.Kill(this);
//    }

//    private void GetHurtAir(HitCollider hit, HurtCollider hurt)
//    {
//        if (hurtParticles != null)
//        {
//            hurtParticles.Play();
//        }

//        // Desactivar el HitCollider y HurtCollider
//        if (hitCollider != null)
//        {
//            hitCollider.gameObject.SetActive(false);
//        }
//        if (hurtCollider != null)
//        {
//            hurtCollider.gameObject.SetActive(false);
//        }

//        // Realizar animación de muerte y movimiento hacia arriba
//        Die();
//    }

//    private void Die()
//    {
//        // Movimiento hacia arriba (salir despedido)
//        transform.DOMoveY(transform.position.y + deathForce, 0.5f).SetEase(Ease.OutQuad);
//        // Reducción de tamaño (como si el enemigo se desintegrara)
//        transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InBack).OnComplete(() =>
//        {
//            Destroy(gameObject); // Elimina el enemigo del juego
//        });
//    }

//    private void MoveInDirection()
//    {
//        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
//    }

//    // Manejo del escape
//    private void HandleEscape()
//    {
//        moveDirection = new Vector3(0, 1, 1).normalized; // Movimiento hacia arriba y hacia atrás
//        transform.Translate(moveDirection * escapeSpeed * Time.deltaTime);
//    }

//    // Manejo del ataque
//    private void HandleAttack()
//    {
//        attackTimer += Time.deltaTime;

//        if (attackTimer >= attackDuration)
//        {
//            EndAttack();
//        }
//        else
//        {
//            Vector3 directionToPlayer = (player.position - transform.position).normalized;
//            transform.Translate(directionToPlayer * attackSpeed * Time.deltaTime);
//        }
//    }

//    // Detección del jugador para iniciar el ataque
//    private void DetectPlayerForAttack()
//    {
//        if (player != null)
//        {
//            float detectionRange = attackRange + extraDetectionRange;
//            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

//            if (distanceToPlayer <= detectionRange)
//            {
//                StartAttack();
//            }
//        }
//    }

//    // Inicia el ataque
//    private void StartAttack()
//    {
//        isAttacking = true;
//        attackTimer = 0f;

//        if (hitCollider != null)
//        {
//            hitCollider.gameObject.SetActive(true);
//        }
//    }

//    // Finaliza el ataque
//    private void EndAttack()
//    {
//        isAttacking = false;

//        if (hitCollider != null)
//        {
//            hitCollider.gameObject.SetActive(false);
//        }

//        StartFleeForever();
//    }

//    // Comienza la huida
//    private void StartFleeForever()
//    {
//        isFleeingForever = true;
//        isEscaping = true;
//        escapeTimer = 0f;

//        if (hitCollider != null)
//        {
//            hitCollider.gameObject.SetActive(false);
//        }
//    }

//    // Manejo de colisión cuando el enemigo golpea al player
//    private void OnHitDelivered(HitCollider hit, HurtCollider hurt)
//    {
//        if (hurt.gameObject.CompareTag("Player"))
//        {
//            StartFleeForever();
//        }
//    }
//}
using DG.Tweening;
using UnityEngine;

public class AirEnemyController : MonoBehaviour
{
    // Variables públicas para controlar el movimiento y ataque
    public float moveSpeed = 5f;
    public float escapeSpeed = 7f;
    public float escapeHeight = 5f;
    public float escapeDuration = 2f;
    public Transform player;
    public HitCollider hitCollider; // Este es el HitCollider que se va a activar

    // Estado interno del enemigo
    private Vector3 moveDirection = Vector3.back;
    private bool isEscaping = false;
    private bool isAttacking = false;
    private bool isFleeingForever = false;
    private float attackTimer = 0f;
    private float escapeTimer = 0f;

    // Configuración de ataque
    public float attackRange = 10f;
    public float attackDuration = 5f;
    public float attackSpeed = 10f;
    public float extraDetectionRange = 2f;

    // Componentes para detección de colisiones
    private HurtCollider hurtCollider;

    // Configuración de muerte y efectos visuales
    public float deathForce = 5f;
    private bool isDead = false;
    public ParticleSystem hurtParticles;

    // Método Awake para inicialización temprana
    void Awake()
    {
        // Inicialización del HitCollider
        if (hitCollider != null)
        {
            hitCollider.gameObject.SetActive(false); // Inicialmente desactivado
            hitCollider.onHitDelivered.AddListener(OnHitDelivered); // Escucha de la colisión
        }

        // Inicialización de HurtCollider
        hurtCollider = GetComponentInChildren<HurtCollider>();
    }

    private void OnEnable()
    {
        hurtCollider.onHitReceived.AddListener(GetHurtAir);
    }

    // Método Update se ejecuta en cada frame
    void Update()
    {
        if (isFleeingForever)
        {
            HandleEscape();
        }
        else if (isAttacking)
        {
            HandleAttack();
        }
        else
        {
            MoveInDirection();
            DetectPlayerForAttack();
        }
    }

    private void OnDisable()
    {
        hurtCollider.onHitReceived.RemoveListener(GetHurtAir);
        DOTween.Kill(this);
    }

    private void GetHurtAir(HitCollider hit, HurtCollider hurt)
    {
        if (hurtParticles != null)
        {
            hurtParticles.Play();
        }

        // Desactivar el HitCollider y HurtCollider
        if (hitCollider != null)
        {
            hitCollider.gameObject.SetActive(false);
        }
        if (hurtCollider != null)
        {
            hurtCollider.gameObject.SetActive(false);
        }

        // Realizar animación de muerte y movimiento hacia arriba
        Die();
    }

    private void Die()
    {
        // Movimiento hacia arriba (salir despedido)
        transform.DOMoveY(transform.position.y + deathForce, 0.5f).SetEase(Ease.OutQuad);
        // Reducción de tamaño (como si el enemigo se desintegrara)
        transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InBack).OnComplete(() =>
        {
            Destroy(gameObject); // Elimina el enemigo del juego
        });
    }

    private void MoveInDirection()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    // Manejo del escape
    private void HandleEscape()
    {
        moveDirection = new Vector3(0, 1, 1).normalized; // Movimiento hacia arriba y hacia atrás
        transform.Translate(moveDirection * escapeSpeed * Time.deltaTime);
    }

    // Manejo del ataque
    private void HandleAttack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDuration)
        {
            EndAttack();
        }
        else
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            transform.Translate(directionToPlayer * attackSpeed * Time.deltaTime);
        }
    }

    // Detección del jugador para iniciar el ataque
    private void DetectPlayerForAttack()
    {
        if (player != null)
        {
            float detectionRange = attackRange + extraDetectionRange;
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange && !isAttacking)
            {
                StartAttack();  // Comienza el ataque de inmediato si el jugador está en rango
            }
        }
    }

    // Inicia el ataque
    private void StartAttack()
    {
        isAttacking = true;
        attackTimer = 0f;

        // Activa el HitCollider inmediatamente
        if (hitCollider != null && !hitCollider.gameObject.activeSelf)
        {
            hitCollider.gameObject.SetActive(true);
        }
    }

    // Finaliza el ataque
    private void EndAttack()
    {
        isAttacking = false;

        // Desactiva el HitCollider después de un tiempo de ataque
        if (hitCollider != null)
        {
            hitCollider.gameObject.SetActive(false);
        }

        StartFleeForever();
    }

    // Comienza la huida
    private void StartFleeForever()
    {
        isFleeingForever = true;
        isEscaping = true;
        escapeTimer = 0f;

        // Desactiva el HitCollider para que no se active durante la huida
        if (hitCollider != null)
        {
            hitCollider.gameObject.SetActive(false);
        }
    }

    // Manejo de colisión cuando el enemigo golpea al player
    private void OnHitDelivered(HitCollider hit, HurtCollider hurt)
    {
        if (hurt.gameObject.CompareTag("Player"))
        {
            StartFleeForever();
        }
    }
}

