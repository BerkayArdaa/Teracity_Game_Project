using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerBlock : MonoBehaviour
{
    public float destructionDelay = 3f; // Objelerin yok olma s�resi (saniye)

    void Start()
    {
        // Belirtilen s�re sonra objeyi yok et
        Destroy(gameObject, destructionDelay);
    }
}
