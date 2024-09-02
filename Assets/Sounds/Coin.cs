using System.Collections.Generic;
using UnityEngine;

public class CoinSound : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> coin; 

    private AudioSource audioSource;
    private Dictionary<GameObject, bool> coinStatus; //Ses ba�lad� m� kontrol�

    private void Start()
    {
       
        audioSource = GetComponent<AudioSource>();

        //Coin durumunu ba�lat
        coinStatus = new Dictionary<GameObject, bool>();
        foreach (var enemy in coin)
        {
            coinStatus[enemy] = true;  // Coinler Toplanmad�.
        }

        // Oyun ba�lang�c�nda sesi sustur
        if (audioSource != null)
        {
            audioSource.mute = true;
        }
    }

    private void Update()
    {
        // Xoin durumu kontrol�
        CheckCoins();
    }

    private void CheckCoins()
    {
        List<GameObject> toRemove = new List<GameObject>();

        foreach (var exist in coinStatus.Keys)
        {
            // coin yok olduysa
            if (exist == null && coinStatus[exist])
            {
                PlaySound(); // Ses �al
                toRemove.Add(exist); // Bu d��man� daha sonra silmek �zere listeye ekle
            }
        }

        // Silinecek d��manlar� i�aretle
        foreach (var enemy in toRemove)
        {
            coinStatus[enemy] = false;
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
