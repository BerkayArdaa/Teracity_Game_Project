using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField]
    public Transform player; // Player'ýn Transform'u
    [SerializeField]
    public GameObject[] projectiles; // Fýrlatýlacak objeler
    [SerializeField]
    public GameObject[] differentProjectiles; // Farklý oluþturulacak objeler
    public float moveSpeed = 2f; // Boss'un hareket hýzý
    public float projectileSpeed = 5f; // Fýrlatýlacak objelerin hýzý
    public float fireRate = 2f; // Fýrlatma aralýðý
    public float differentProjectilesSpawnRate = 5f; // Farklý objelerin doðma süresi
    public float attackRange = 10f; // Boss'un saldýrý menzili
    public int health = 1000; // Boss'un caný

    private float nextFireTime = 0f;
    private float nextDifferentProjectilesTime = 0f;
    [SerializeField]
    public GameObject objectToDrop;
    void Update()
    {
        // Player'a yönelme
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Oyuncu menzilde mi kontrol et
        if (Vector3.Distance(player.position, transform.position) <= attackRange)
        {
            // Objeleri fýrlatma
            if (Time.time > nextFireTime)
            {
                FireProjectiles();
                nextFireTime = Time.time + fireRate;
            }

            // Farklý objeleri oluþturma
            if (Time.time > nextDifferentProjectilesTime)
            {
                CreateDifferentProjectiles();
                nextDifferentProjectilesTime = Time.time + differentProjectilesSpawnRate;
            }
        }
    }

    void FireProjectiles()
    {
        foreach (GameObject projectile in projectiles)
        {
            GameObject instantiatedProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            Rigidbody2D rb = instantiatedProjectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 shootDirection = (player.position - transform.position).normalized;
                rb.velocity = shootDirection * projectileSpeed;
            }
        }
    }

    void CreateDifferentProjectiles()
    {
        // Farklý objeler belirli mesafelere yerleþtiriliyor
        Vector3[] positions = new Vector3[]
        {
            transform.position + new Vector3(0, 0.5f, 0), // 0.5f yukarý
            transform.position + new Vector3(0, -0.5f, 0) // 0.5f aþaðý
        };

        foreach (Vector3 pos in positions)
        {
            foreach (GameObject projectile in differentProjectiles)
            {
                Instantiate(projectile, pos, Quaternion.identity);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Eðer kýlýç Boss'a deðdiyse
        if (collision.CompareTag("Sword"))
        {
            TakeDamage(10); // Boss'un canýný 10 azalt
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Boss took damage. Current health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Boss died.");
        Vector3 dropPosition = transform.position + new Vector3(0, 0.2f, 0);
        Instantiate(objectToDrop, dropPosition, Quaternion.identity);
        Destroy(gameObject); // Destroy the boss
    }

}
