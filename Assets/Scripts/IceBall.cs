using UnityEngine;

public class IceBall : MonoBehaviour
{
    // Tags or names for objects that should destroy the fireball
    [SerializeField] private string destroyTag1 = "Player";
    [SerializeField] private string destroyTag2 = "Walls";

    private void Start()
    {
        // Destroy the fireball after 2 seconds
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the fireball hit an object with the specified tags
        if (collision.collider.CompareTag(destroyTag1) || collision.collider.CompareTag(destroyTag2))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the fireball entered a trigger with the specified tags
        if (other.CompareTag(destroyTag1) || other.CompareTag(destroyTag2))
        {
            Destroy(gameObject);
        }
    }
}
