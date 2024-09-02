using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private Inventory_P inventory;
    public int i;

    public void Start()
    {
        inventory=GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory_P>();
    }
    public void Update()
    {
        if (transform.childCount <= 0) {
            inventory.isFull[i]=false;
        }
    }
    public void DropItem()
    {
        foreach(Transform child in transform)
        {
            
            GameObject.Destroy(child.gameObject);
            child.GetComponent<Spawn>().SpawnDroppedItem();
        }
    }
}
