using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class CollideableObject : MonoBehaviour
{
    private Collider2D collider;
    [SerializeField]
    private ContactFilter2D filter;
    private List<Collider2D> colliders= new List<Collider2D>(1);

    protected virtual void Start()
    {
        collider = GetComponent<Collider2D>();

    }
    protected virtual void Update()
    {
        collider.OverlapCollider(filter,colliders);
        foreach (var o in colliders) {
        OnCollidded(o.gameObject);
           
        }
    }
    protected virtual void OnCollidded(GameObject collidedObj)
    {
        Debug.Log("Collided with" + collidedObj.name);
    }    
}
