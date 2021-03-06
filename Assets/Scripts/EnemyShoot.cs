using UnityEngine;
using NaughtyAttributes;

public class EnemyShoot : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform cannon;
    public float repeatRate;
    [ReadOnly] public bool reparado;
    public GameObject garraPrefab;
    [ReadOnly] public GameObject roof;
    [SerializeField] private LayerMask playerLayer;
    private float timerDisparo;
    private new Collider collider;
    private new Rigidbody rigidbody;
    private bool dead;
    [SerializeField] private Animator animator;

    private GameObject garra;

    private void Start()
    {
        reparado = false;
        timerDisparo = 0;
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 dirjugador = new Vector3(cannon.position.x - gameObject.transform.position.x, 0f, 0f);

        Debug.DrawRay(transform.position, dirjugador * 10f, Color.blue);
        if (reparado == false)
        {
            PlayerOnSight = Physics.Raycast(transform.position, dirjugador, out hit, 100f, playerLayer) && hit.collider.CompareTag("Player");

            if (PlayerOnSight)
            {
                timerDisparo = timerDisparo + Time.deltaTime;
                if (timerDisparo > repeatRate)
                {
                    animator.SetTrigger("ataco");
                    Instantiate(laserPrefab, cannon.position, transform.rotation);
                    timerDisparo = 0;
                }
            }
        }
    }

    [ShowNativeProperty]
    public bool PlayerOnSight { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == garra)
            transform.SetParent(other.transform);

        if (other.CompareTag("techo"))
            Destroy(gameObject, 3);
    }

    public void OnDeath()
    {
        if (!dead)
        {
            dead = true;

            collider.isTrigger = true;
            rigidbody.isKinematic = true;
            Vector3 arriba = new Vector3(transform.position.x, roof.transform.position.y + 30f, 0f);
            reparado = true;
            animator.SetTrigger("reparado");
            garra = Instantiate(garraPrefab, arriba, transform.rotation);
            garra.GetComponent<GarraController>().enemyToGrab = gameObject;
        }
    }
}
