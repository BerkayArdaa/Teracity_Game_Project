using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTr : MonoBehaviour
{
    public GameObject player;  // Player objesine referans

    private void Update()
    {
        // Oyuncu objesinin olup olmad���n� kontrol et
        if (player == null)
        {
            ScoreManager.instance.GameOver();
        }
    }
}
