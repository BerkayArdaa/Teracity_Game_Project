using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minions : MonoBehaviour
{
    public int scoreValue = 1;
    [SerializeField]
    private GameObject objectToDrop;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private float moveSpeed = 2f;
    [SerializeField]
    private float followDistance = 5f;
    [SerializeField]
    private int health = 20;  // Minions daha az cana sahip olabilir

    private bool isTriggered = false;
    private EnemyManager enemyManager;

    void Start()
    {
        enemyManager = GameObject.FindObjectOfType<EnemyManager>();
      
    }

    void Update()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        // player-Minions mesafe hesaplama
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (player.position.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (player.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (distanceToPlayer <= followDistance)
        {
            isTriggered = true;
          
            // yön hesapla
            Vector2 direction = (player.position - transform.position).normalized;

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
        // Eðer kýlýç miniona deðdiyse
        if (collision.CompareTag("Sword"))
        {
            TakeDamage(10); // Minions caný daha az azalabilir
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Minion took damage. Current health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        ScoreManager.instance.AddScore(scoreValue);
        Debug.Log("Minion died.");
        Vector3 dropPosition = transform.position + new Vector3(0, 0.2f, 0);
        Instantiate(objectToDrop, dropPosition, Quaternion.identity);
        
        Destroy(gameObject); // Minion ortadan kalksýn
    }
}
