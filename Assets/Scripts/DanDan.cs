using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanDan : MonoBehaviour
{
    void Start()
    {
        // 5 saniye sonra objeyi yok et
        Destroy(gameObject, 3f);
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // E�er Player tagine sahip bir objeye �arparsa, objeyi hemen yok et
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
