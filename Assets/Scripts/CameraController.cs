using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform z_Player;
    [SerializeField]
    private float z_boundX = 0.15f;
    [SerializeField]
    private float z_boundY = 0.2f;

    private void LateUpdate()
    {
        FollowAPlayer();
    }

    //kamera oyuncuyu takip etsin
    private void FollowAPlayer()
    {
        Vector2 moveDirection = Vector2.zero;

        float deltaX = z_Player.position.x - transform.position.x;
        float deltaY = z_Player.position.y - transform.position.y;

        if (deltaX > z_boundX || deltaX < -z_boundX)
        {
            if (z_Player.position.x > transform.position.x)
            {
                moveDirection.x = deltaX - z_boundX;
            }
            else
            {
                moveDirection.x = deltaX + z_boundX;
            }
        }

        if (deltaY > z_boundY || deltaY < -z_boundY)
        {
            if (z_Player.position.y > transform.position.y)
            {
                moveDirection.y = deltaY - z_boundY;
            }
            else
            {
                moveDirection.y = deltaY + z_boundY;
            }
        }

        transform.position += new Vector3(moveDirection.x, moveDirection.y, 0);
    }
}
