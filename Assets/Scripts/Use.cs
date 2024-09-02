using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Use : MonoBehaviour
{
    public int healthIncreaseAmount = 20;
    public float speedMultiplier = 2f;
    public float shieldIncreaseAmount = 50f; // Amount to increase the shield
    [SerializeField]
    public PlayerMovement playerMovement;

    public GameObject button;

    private void Start()
    {
        if (playerMovement == null)
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
        }
    }

    public void UseHeartPot()
    {
        if (playerMovement != null)
        {
            playerMovement.IncreaseHP(healthIncreaseAmount);
            RemoveButtonFromInventory();
        }
        else
        {
            Debug.LogError("PlayerMovement component not found.");
        }
    }

    public void UseBuffPot()
    {
        if (playerMovement != null)
        {
            playerMovement.IncreaseSpeed(speedMultiplier);
            RemoveButtonFromInventory();
        }
        else
        {
            Debug.LogError("PlayerMovement component not found.");
        }
    }

    public void UseShieldPot()
    {
        if (playerMovement != null)
        {
            playerMovement.IncreaseShield(shieldIncreaseAmount);
            RemoveButtonFromInventory();
        }
        else
        {
            Debug.LogError("PlayerMovement component not found.");
        }
    }

    private void RemoveButtonFromInventory()
    {
        if (button != null)
        {
            Destroy(button);
        }
        else
        {
            Debug.LogError("Button GameObject not assigned.");
        }
    }
}
