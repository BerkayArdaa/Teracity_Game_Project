using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObj : CollideableObject
{
    [SerializeField]
    List<GameObject> droppedItems;
    [SerializeField]
    public Transform chest;

    protected override void OnCollidded(GameObject collidedObj)
    {
        if (Input.GetKey(KeyCode.E))
        {
            OnInterract();
        }
    }

    private void OnInterract()
    {
        Debug.Log("Interaction successful!");

        // %50 þans
        bool shouldDropItem = Random.value > 0.5f;

        if (shouldDropItem && droppedItems.Count > 0)
        {
            int randomIndex = Random.Range(0, droppedItems.Count);
            GameObject selectedItem = droppedItems[randomIndex];

            Vector2 playerPos = new Vector2(chest.position.x, chest.position.y + chest.position.z + 0.21f);

            // sandýk yok olsun
            Destroy(gameObject);
            Instantiate(selectedItem, playerPos, Quaternion.identity);

            Debug.Log("Item dropped: " + selectedItem.name);
        }
        else
        {
            Debug.Log("No item dropped from the chest.");
        }
    }
}
