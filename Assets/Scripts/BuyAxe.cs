using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    public GameObject[] axes; // Drag and drop axe GameObjects in the Inspector
    private int currentAxe = 0; // Index to determine which axe to activate
    private bool isPlayerNear = false;

    private PlayerMovement player; // Reference to PlayerMovement script

    void Start()
    {
        // Assuming the player has a tag "Player" and PlayerMovement component attached
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
            Debug.Log("Player is near the item");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            Debug.Log("Player left the item");
        }
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            BuyAxe();
        }
    }

    void BuyAxe()
    {
        if (player != null && player.score >= 200) // Check if the player has enough score
        {
            if (currentAxe < axes.Length)
            {
                axes[currentAxe].SetActive(true);
                currentAxe++;
                player.DecreaseScore(200); // Decrease player's score by 200
                Debug.Log("Axe purchased and activated");
            }
            else
            {
                Debug.Log("All axes are already active");
            }
        }
        else
        {
            Debug.Log("Not enough score to buy an axe");
        }
    }
}
