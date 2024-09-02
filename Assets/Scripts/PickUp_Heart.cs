using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PickUp : MonoBehaviour
{
    private Inventory_P inventory;
    public GameObject itemButton;

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory_P>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i<inventory.slots.Length; i++)
            {
                if (inventory.isFull[i] == false)
                {
                    
                    inventory.isFull[i] = true;
                    Instantiate(itemButton, inventory.slots[i].transform,false);
                    Destroy(gameObject);
                    break;

                }

            }
        }
        
    }
}
