using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icewizard : MonoBehaviour
{
    public int scoreValue = 1;
    [SerializeField]
    private Transform objectToDrop;
    [SerializeField]
    private Transform enemy;
    [SerializeField]
    private Transform enemyItself;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private float moveSpeed = 2f;
    [SerializeField]
    private float followDistance = 5f;
    [SerializeField]
    private int health = 50;
  
    private bool isTriggered = false;

    [SerializeField]
    GameObject Object;
    [SerializeField]
    GameObject Fireball;
    private float fireballRange = 1f;
    private float fireballSpeed = 1f;
    private float fireballCooldown = 2f;
    private float fireballTimer = 0f;
    private EnemyManager enemyManager;

    public void Start()
    {
        enemyManager = GameObject.FindObjectOfType<EnemyManager>();
    }

    private void Update()
    {
        MoveTowardsEnemy();
        HandleFireballThrow();
    }

    private void MoveTowardsEnemy()
    {
        // player-Enemy mesafe hesaplama
        float distanceToEnemy = Vector2.Distance(transform.position, enemy.position);

        if (player.position.x - enemyItself.position.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (player.position.x - enemyItself.position.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (distanceToEnemy <= followDistance)
        {
            isTriggered = true;
        
            // yön hesapla
            Vector2 direction = (enemy.position - transform.position).normalized;

            // Player'a yönel
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
        else
        {
            isTriggered = false;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Eðer kýlýç düþmana deðdiyse
        if (collision.CompareTag("Sword"))
        {
            TakeDamage(20); // caný 10 azalt
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy took damage. Current health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        ScoreManager.instance.AddScore(scoreValue);

        Debug.Log("Enemy died.");
        Vector3 dropPosition = transform.position + new Vector3(0, 0.2f, 0);
        Instantiate(objectToDrop, dropPosition, Quaternion.identity);
        enemyManager.EnemyKilled();
        Destroy(Object);
        Destroy(gameObject); // enemy ortadan kalksýn
    }

    private void HandleFireballThrow()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Check if player is within the fireball range
        if (distanceToPlayer <= fireballRange && fireballTimer <= 0f)
        {
            ThrowFireball();
            fireballTimer = fireballCooldown; // Reset cooldown timer
        }

        // Reduce the cooldown timer over time
        if (fireballTimer > 0f)
        {
            fireballTimer -= Time.deltaTime;
        }
    }

    private void ThrowFireball()
    {
        // Calculate direction to the player's current position
        Vector2 direction = (player.position - transform.position).normalized;

        // Offset position to prevent collision with the wizard itself
        Vector3 fireballStartPosition = transform.position + (Vector3)direction * 0.05f;

        // Create a fireball instance at the offset position
        GameObject fireball = Instantiate(Fireball, fireballStartPosition, Quaternion.identity);
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();

        // Set the fireball's velocity
        rb.velocity = direction * fireballSpeed;
    }
}
