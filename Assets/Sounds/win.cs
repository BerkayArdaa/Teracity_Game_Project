using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> enemies; // Liste halinde düþmanlar

    private AudioSource audioSource;
    private bool soundPlayed = false; // Sesin çalýnýp çalýnmadýðýný kontrol etmek için

    private void Start()
    {
        // AudioSource component'ini bu GameObject'ten al
        audioSource = GetComponent<AudioSource>();

        // Baþlangýçta AudioSource'u sessize al
        if (audioSource != null)
        {
            audioSource.mute = true;
        }
    }

    private void Update()
    {
        // Düþmanlarýn durumunu sürekli kontrol et
        CheckEnemies();
    }

    private void CheckEnemies()
    {
        // Tüm düþmanlar yok oldu mu diye kontrol et
        bool allEnemiesDead = true;
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                allEnemiesDead = false; // Eðer hala canlý düþman varsa, bu deðiþkeni false yap
                break;
            }
        }

        // Eðer tüm düþmanlar yok olduysa ve ses daha önce çalýnmadýysa, sesi gecikmeli olarak çal
        if (allEnemiesDead && !soundPlayed)
        {
            Invoke("PlaySound", 3f); // 3 saniye sonra PlaySound metodunu çaðýr
            soundPlayed = true; // Ses çaldý olarak iþaretle
        }
    }

    private void PlaySound()
    {
        // AudioSource var mý kontrol et
        if (audioSource != null)
        {
            audioSource.mute = false; // Sessizliði kaldýr
            audioSource.Play(); // Ses çal
            Debug.Log("Victory sound is playing");
        }
    }
}
