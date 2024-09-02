using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinMovement : MonoBehaviour
{
    public int scoreValue = 1;
    [SerializeField]
    private GameObject objectToDrop;
    [SerializeField]
    private Transform enemy;
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
    private EnemyManager enemyManager;

    private void Start()
    {
        enemyManager = GameObject.FindObjectOfType<EnemyManager>();
    }
    private void Update()
    {
        MoveTowardsEnemy();
    }

    private void MoveTowardsEnemy()
    {
        float distanceToEnemy = Vector2.Distance(transform.position, player.position); // Hedef oyuncu olmalý

        // Yön deðiþtirme
        transform.localScale = new Vector3(player.position.x > transform.position.x ? 1 : -1, 1, 1);

        // Oyuncuya doðru hareket
        if (distanceToEnemy <= followDistance)
        {
            isTriggered = true;
            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
        else
        {
            isTriggered = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword"))
        {
            TakeDamage(20);
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
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
        Destroy(gameObject);
    }
}
