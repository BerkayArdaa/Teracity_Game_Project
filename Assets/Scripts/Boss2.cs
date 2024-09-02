using System.Collections;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public GameObject firstObjectPrefab;
    public GameObject secondObjectPrefab;
    public Transform player;
    public float detectionRange = 10f;
    public float moveSpeed = 2f;
    public float spawnInterval = 3f;
    public float spawnRange = 3f;
    public int minSpawnCount = 5;
    public int maxSpawnCount = 10;
    public float secondObjectDelay = 1f; // Ýkinci objelerin gecikme süresi
    public int health = 100; // Boss'un caný

    private float spawnTimer;
    private Animator z_BossAnimator;
    private EnemyManager enemyManager;
    [SerializeField]
    public GameObject objectToDrop;

    private void Start()
    {
        enemyManager = GameObject.FindObjectOfType<EnemyManager>();
        z_BossAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // Oyuncu menzildeyse hareket et ve objeler oluþtur
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            // Boss'un yönünü oyuncuya doðru ayarla
            AdjustDirection();

            // Oyuncuya doðru hareket et
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // Obje oluþturma iþlemini gerçekleþtir
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                spawnTimer = 0f;
                SpawnObjects();
            }
        }
    }

    void AdjustDirection()
    {
        // Oyuncu boss'un saðýndaysa saða, solundaysa sola baksýn
        if (player.position.x < transform.position.x)
        {
            // Sola bakacak þekilde yönünü ayarla
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            // Saða bakacak þekilde yönünü ayarla
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void SpawnObjects()
    {
        // Rastgele bir sayýda ilk objeleri oluþtur
        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount);
        Vector2[] spawnPositions = new Vector2[spawnCount];

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRange;
            Instantiate(firstObjectPrefab, spawnPosition, Quaternion.identity);
            spawnPositions[i] = spawnPosition;
        }

        // Ýkinci objelerin oluþumunu bir saniye geciktir
        StartCoroutine(SpawnSecondObjectsAfterDelay(spawnPositions));
    }

    IEnumerator SpawnSecondObjectsAfterDelay(Vector2[] spawnPositions)
    {
        yield return new WaitForSeconds(secondObjectDelay);

        // Oluþan her objenin olduðu yerde ikinci objeyi oluþtur
        foreach (Vector2 position in spawnPositions)
        {
            Instantiate(secondObjectPrefab, position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Eðer kýlýç boss'a deðdiyse
        if (collision.CompareTag("Sword"))
        {
            TakeDamage(20); // caný 20 azalt
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
        // Boss'un ölümü sýrasýnda yapýlacak iþlemler
        Debug.Log("Boss died.");
        Vector3 dropPosition = transform.position + new Vector3(0, 0.2f, 0);
        Instantiate(objectToDrop, dropPosition, Quaternion.identity);
        enemyManager.EnemyKilled();
        Destroy(gameObject); // Boss yok olsun
    }
}
