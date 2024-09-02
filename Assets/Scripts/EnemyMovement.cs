using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public int scoreValue = 1;
    [SerializeField]
    private GameObject objectToDrop;
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
    private Animator z_EnAnimator;
    private bool isTriggered = false;
    private EnemyManager enemyManager;

    [SerializeField]
    GameObject Object;



    public void Start()
    {
        enemyManager = GameObject.FindObjectOfType<EnemyManager>();
        z_EnAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        MoveTowardsEnemy();
    }

    private void MoveTowardsEnemy()
    {
        // player-Enemy meseafe hesaplama
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
            z_EnAnimator.SetBool("isTriggered", isTriggered);
            // yön hesapla
            Vector2 direction = (enemy.position - transform.position).normalized;

            // Player'a yönel
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
        else
        {
            isTriggered = false;
            z_EnAnimator.SetBool("isTriggered", isTriggered);
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
}
