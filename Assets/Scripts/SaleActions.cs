using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaleActions : MonoBehaviour
{
    [SerializeField]
    GameObject Player;

    // The target position to teleport the player to
    [SerializeField]
    Vector3 targetPosition = new Vector3(-46f, -3.45f, 0);
    Vector3 targetPosition2 = new Vector3(-35.21f, 7.26f, 0);

    public void GoToSale()
    {
        // Set the player's position to the target position
        if (Player != null)
        {
            Player.transform.position = targetPosition;
        }
        else
        {
            Debug.LogError("Player GameObject is not assigned.");
        }
    }
    public void RemoveSale()
    {
        // Set the player's position to the target position
        if (Player != null)
        {
            Player.transform.position = targetPosition2;
        }
        else
        {
            Debug.LogError("Player GameObject is not assigned.");
        }
    }
}
