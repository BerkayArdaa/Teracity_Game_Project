using System.Collections.Generic;
using UnityEngine;

public class CoinSound : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> coin; 

    private AudioSource audioSource;
    private Dictionary<GameObject, bool> coinStatus; //Ses baþladý mý kontrolü

    private void Start()
    {
       
        audioSource = GetComponent<AudioSource>();

        //Coin durumunu baþlat
        coinStatus = new Dictionary<GameObject, bool>();
        foreach (var enemy in coin)
        {
            coinStatus[enemy] = true;  // Coinler Toplanmadý.
        }

        // Oyun baþlangýcýnda sesi sustur
        if (audioSource != null)
        {
            audioSource.mute = true;
        }
    }

    private void Update()
    {
        // Xoin durumu kontrolü
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
                PlaySound(); // Ses çal
                toRemove.Add(exist); // Bu düþmaný daha sonra silmek üzere listeye ekle
            }
        }

        // Silinecek düþmanlarý iþaretle
        foreach (var enemy in toRemove)
        {
            coinStatus[enemy] = false;
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
