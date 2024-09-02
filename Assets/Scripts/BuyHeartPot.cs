using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToSpawn; // The object to spawn

    private bool isPlayerNear = false;
    private PlayerMovement player; // Reference to PlayerMovement script

    void Start()
    {
        // Find the player GameObject and get the PlayerMovement component
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerMovement>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure the player has a "Player" tag
        {
            isPlayerNear = true;
            Debug.Log("Player is near the spawner");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            Debug.Log("Player left the spawner");
        }
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
           
            player.UpdateScoreText();
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        if (player != null && player.score >= 50) // Check if the player has enough score
        {
            Vector3 spawnPosition = player.transform.position + Vector3.up * 0.2f; // Calculate spawn position 2 units above the player
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity); // Spawn the object
            Debug.Log("Object spawned above the player");
            player.score-=50;
        }
        else
        {
            Debug.Log("Not enough score to spawn the object");
        }
    }
}
