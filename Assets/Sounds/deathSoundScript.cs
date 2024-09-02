using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSoundScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> enemies; // Liste halinde düþmanlar

    private AudioSource audioSource;
    private Dictionary<GameObject, bool> enemyStatuses; // Düþmanlarýn canlý/ölü durumunu takip etmek için bir sözlük

    private void Start()
    {
        // AudioSource component'ini bu GameObject'ten al
        audioSource = GetComponent<AudioSource>();

        // Düþmanlarýn durumlarýný baþlat
        enemyStatuses = new Dictionary<GameObject, bool>();
        foreach (var enemy in enemies)
        {
            enemyStatuses[enemy] = true;  // Tüm düþmanlarý baþlangýçta canlý olarak iþaretle
        }

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
        List<GameObject> toRemove = new List<GameObject>();

        foreach (var enemy in enemyStatuses.Keys)
        {
            // Düþman null ise ve daha önce öldü olarak iþaretlenmemiþse
            if (enemy == null && enemyStatuses[enemy])
            {
                PlaySound(); // Ses çal
                toRemove.Add(enemy); // Bu düþmaný daha sonra silmek üzere listeye ekle
            }
        }

        // Silinecek düþmanlarý iþaretle
        foreach (var enemy in toRemove)
        {
            enemyStatuses[enemy] = false;
        }
    }

    private void PlaySound()
    {
        // AudioSource var mý kontrol et
        if (audioSource != null)
        {
            audioSource.mute = false; // Sessizliði kaldýr
            audioSource.Play(); // Ses çal
            Debug.Log("Sound is playing");
        }
    }
}
