using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerBlock : MonoBehaviour
{
    public float destructionDelay = 3f; // Objelerin yok olma süresi (saniye)

    void Start()
    {
        // Belirtilen süre sonra objeyi yok et
        Destroy(gameObject, destructionDelay);
    }
}
