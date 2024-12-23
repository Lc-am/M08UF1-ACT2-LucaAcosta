using DG.Tweening;
using System.Threading;
using UnityEngine;
using System.Collections;

public class GroundEnemyController : MonoBehaviour
{
    // Movimiento
    
    [SerializeField] public float velocity = 5f;
    [SerializeField] public float verticalVelocityOnGrounded = -1f;
    float gravity = -9.8f;
    float verticalVelocity = 0f;
    [SerializeField] float timer = 0;

    CharacterController enemyController;
    
    // HitHurt

    [SerializeField] HitCollider hitCollider;
    HurtCollider hurtCollider;

    // Daño y muerte

    [SerializeField] PlayerDetected playerDetected;
    [SerializeField] int lives = 3;
    public ParticleSystem hurtParticles;

    private Renderer objRenderer;
    public Material originalMaterial;
    public Material temporaryMaterial;
    public float duration = 2.0f;
    private float timerDaño = 0f;
    private bool daño = false;
    public float deathForce = 10f;

    private void Start()
    {
         objRenderer = GetComponent<Renderer>();
    
         objRenderer.material = originalMaterial;
    }

    private void Awake()
    {
        enemyController = GetComponent<CharacterController>();
        hurtCollider = GetComponent<HurtCollider>();
    }

    private void OnEnable()
    {
        hurtCollider.onHitReceived.AddListener(GetHurt);
    }

    // Modificar update
    void Update()
    {
        // Movimiento
        enemyController.Move((-Vector3.forward * velocity + Vector3.up * verticalVelocity) * Time.deltaTime);

        // Deteccion de suelo

        if(enemyController.isGrounded)
        {
            verticalVelocity = verticalVelocityOnGrounded;
        }

        verticalVelocity += gravity * Time.deltaTime;


        // Cuando el Enemy detecta al player

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

        if(daño)
        {
            timer += Time.deltaTime;

            if (timer >= duration)
            {
                objRenderer.material = originalMaterial;
                daño = false;
                timer = 0f; 
            }
        }
    }

    private void OnDisable()
    {
        hurtCollider.onHitReceived.RemoveListener(GetHurt);
        DOTween.Kill(this);
    }

    private void GetHurt(HitCollider hit, HurtCollider hurt)
    {
        if(lives > 0)
        {
            lives--;

            objRenderer.material = temporaryMaterial;
            daño = true;
            timerDaño = 0f;

            Particle();
        }
        else
        {
            Die();
        }
    }

    private void Particle()
    {

        StopCoroutine("DisableParticlesAfterPlay");

        hurtParticles.gameObject.SetActive(true);
        hurtParticles.Play();

        StartCoroutine(DisableParticlesAfterPlay());
        

    }

    private IEnumerator DisableParticlesAfterPlay()
    {
        if (hurtParticles == null)
            yield break;

        while (hurtParticles.isPlaying)
        {
            yield return null; 
        }

        hurtParticles.gameObject.SetActive(false);
        
    }

    private void Die()
    {
        hitCollider.gameObject.SetActive(false);  

        StartCoroutine(MoveUpwardsAndDisappear());
    }

    private IEnumerator MoveUpwardsAndDisappear()
    {
        float elapsedTime = 0f;
        float duration = 0.1f;  
        float maxHeight = 50f;   

        while (elapsedTime < duration)
        {
            transform.Translate(Vector3.up * (maxHeight / duration) * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.DOScale(Vector3.zero, 0.5f).OnKill(() =>
        {
            Destroy(gameObject);
        });
    }

}
