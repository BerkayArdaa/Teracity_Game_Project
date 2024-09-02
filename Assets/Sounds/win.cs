using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> enemies; // Liste halinde d��manlar

    private AudioSource audioSource;
    private bool soundPlayed = false; // Sesin �al�n�p �al�nmad���n� kontrol etmek i�in

    private void Start()
    {
        // AudioSource component'ini bu GameObject'ten al
        audioSource = GetComponent<AudioSource>();

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
        // T�m d��manlar yok oldu mu diye kontrol et
        bool allEnemiesDead = true;
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                allEnemiesDead = false; // E�er hala canl� d��man varsa, bu de�i�keni false yap
                break;
            }
        }

        // E�er t�m d��manlar yok olduysa ve ses daha �nce �al�nmad�ysa, sesi gecikmeli olarak �al
        if (allEnemiesDead && !soundPlayed)
        {
            Invoke("PlaySound", 3f); // 3 saniye sonra PlaySound metodunu �a��r
            soundPlayed = true; // Ses �ald� olarak i�aretle
        }
    }

    private void PlaySound()
    {
        // AudioSource var m� kontrol et
        if (audioSource != null)
        {
            audioSource.mute = false; // Sessizli�i kald�r
            audioSource.Play(); // Ses �al
            Debug.Log("Victory sound is playing");
        }
    }
}
