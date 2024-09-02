using UnityEngine;

public class RotateAround : MonoBehaviour
{

    public Transform target; // The center of rotation and the object to follow
    public float rotationSpeed = 50f; // Rotation speed around the target
    public float followSpeed = 5f; // Speed to move towards the target

    void Update()
    {
        if (target != null)
        {
            // Rotate around the target at 'rotationSpeed' degrees per second
            transform.RotateAround(target.position, Vector3.forward, rotationSpeed * Time.deltaTime);

            // Move towards the target at 'followSpeed' units per second
            transform.position = Vector3.MoveTowards(transform.position, target.position, followSpeed * Time.deltaTime);
        }
    }
}
