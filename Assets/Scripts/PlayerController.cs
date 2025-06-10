using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float originalSpeed = 5f;
    [SerializeField] private float boostedSpeed = 10f;

    private float currentSpeed;

    private Rigidbody rb;
    private Vector3 move;
    private bool Shield = false;

    public int playerHealth = 3;

    public GameObject bulletPrefab;
    public float bulletSpeed = 30f;
    public Transform firePoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = originalSpeed;
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical"); // Changed from moveY to moveZ for XZ plane movement
        move = new Vector3(moveX, 0f, moveZ);
        move.Normalize();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        HandleRotation();
    }

    void HandleRotation()
    {
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.2f);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = move * currentSpeed;
    }

    private IEnumerator SpeedBoost()
    {
        currentSpeed = boostedSpeed;
        yield return new WaitForSeconds(30f);
        currentSpeed = originalSpeed;
    }

    private IEnumerator ShieldBoost()
    {
        Shield = true;
        yield return new WaitForSeconds(45f);
        Shield = false;
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
    }

    void OnTriggerEnter(Collider other) // Changed from OnTriggerEnter2D
    {
        if (other.CompareTag("Shield"))
        {
            Destroy(other.gameObject);
            StartCoroutine(ShieldBoost());
        }
        else if (other.CompareTag("SpeedBoost"))
        {
            Destroy(other.gameObject);
            StartCoroutine(SpeedBoost());
        }
    }

    void OnCollisionEnter(Collision collision) // Changed from OnCollisionEnter2D
    {
        if (collision.gameObject.CompareTag("Enemy") && !Shield)
        {
            TakeDamage(1);
            Debug.Log(playerHealth);
            if (playerHealth < 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        Rigidbody rbb = bullet.GetComponent<Rigidbody>();
        rbb.linearVelocity = transform.forward * bulletSpeed;
    }
}