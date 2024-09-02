using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSoundScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> enemies; // Liste halinde d��manlar

    private AudioSource audioSource;
    private Dictionary<GameObject, bool> enemyStatuses; // D��manlar�n canl�/�l� durumunu takip etmek i�in bir s�zl�k

    private void Start()
    {
        // AudioSource component'ini bu GameObject'ten al
        audioSource = GetComponent<AudioSource>();

        // D��manlar�n durumlar�n� ba�lat
        enemyStatuses = new Dictionary<GameObject, bool>();
        foreach (var enemy in enemies)
        {
            enemyStatuses[enemy] = true;  // T�m d��manlar� ba�lang��ta canl� olarak i�aretle
        }

        // Ba�lang��ta AudioSource'u sessize al
        if (audioSource != null)
        {
            audioSource.mute = true;
        }
    }

    private void Update()
    {
        // D��manlar�n durumunu s�rekli kontrol et
        CheckEnemies();
    }

    private void CheckEnemies()
    {
        List<GameObject> toRemove = new List<GameObject>();

        foreach (var enemy in enemyStatuses.Keys)
        {
            // D��man null ise ve daha �nce �ld� olarak i�aretlenmemi�se
            if (enemy == null && enemyStatuses[enemy])
            {
                PlaySound(); // Ses �al
                toRemove.Add(enemy); // Bu d��man� daha sonra silmek �zere listeye ekle
            }
        }

        // Silinecek d��manlar� i�aretle
        foreach (var enemy in toRemove)
        {
            enemyStatuses[enemy] = false;
        }
    }

    private void PlaySound()
    {
        // AudioSource var m� kontrol et
        if (audioSource != null)
        {
            audioSource.mute = false; // Sessizli�i kald�r
            audioSource.Play(); // Ses �al
            Debug.Log("Sound is playing");
        }
    }
}
