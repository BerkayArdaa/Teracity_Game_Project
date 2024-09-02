using UnityEngine;

public class Sword : MonoBehaviour
{
    public PlayerMovement playerMovement; 

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") && playerMovement.IsAttacking)
        {
            Debug.Log("Kýlýç düþmana deðdi!");
           
            EnemyHealth1 enemy = collider.GetComponent<EnemyHealth1>();
            if (enemy != null)
            {
                enemy.TakeDamage(10); 
            }
        }
    }
}